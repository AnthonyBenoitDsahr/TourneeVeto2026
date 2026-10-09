using System.Collections.Immutable;
using TourneeVeto.Domain.Biosecurity;

namespace TourneeVeto.Domain.Visits;

public enum RegiePractice
{
    Calving,
    DryOff,
    Insemination,
    PregnancyDiagnosis,
    SomaticCellCount
}

public enum RegieActionStatus
{
    Planned,
    Completed,
    Deferred
}

public sealed record RegieAction
{
    public RegieAction(
        string animalId,
        string animalName,
        RegiePractice practice,
        RegieActionStatus status,
        string? note = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(animalId);
        ArgumentException.ThrowIfNullOrWhiteSpace(animalName);

        if (!Enum.IsDefined(practice))
        {
            throw new ArgumentOutOfRangeException(nameof(practice), practice, "The practice is not supported.");
        }

        if (!Enum.IsDefined(status))
        {
            throw new ArgumentOutOfRangeException(nameof(status), status, "The status is not supported.");
        }

        AnimalId = animalId;
        AnimalName = animalName;
        Practice = practice;
        Status = status;
        Note = note;
    }

    public string AnimalId { get; }

    public string AnimalName { get; }

    public RegiePractice Practice { get; }

    public RegieActionStatus Status { get; }

    public string? Note { get; }
}

public sealed record VisitFinding
{
    public VisitFinding(string description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        Description = description;
    }

    public string Description { get; }
}

public sealed record PriorityPractice
{
    public PriorityPractice(BiosecurityQuestion question, Answer answer)
    {
        ArgumentNullException.ThrowIfNull(question);

        Question = question;
        Answer = answer;
    }

    public BiosecurityQuestion Question { get; }

    public Answer Answer { get; }
}

/// <summary>
/// Immutable, non-clinical report synthesis built from the information recorded for a visit.
/// </summary>
public sealed record VisitReportSummary
{
    private VisitReportSummary(
        DateOnly date,
        ImmutableArray<RegieAction> regieActions,
        ImmutableArray<BiosecurityResponse> biosecurityResponses,
        ImmutableArray<BiosecuritySectionScore> biosecuritySectionScores,
        ImmutableArray<VisitFinding> findings,
        ImmutableArray<Guid> photoIds,
        ImmutableArray<PriorityPractice> priorityPractices)
    {
        Date = date;
        RegieActions = regieActions;
        BiosecurityResponses = biosecurityResponses;
        BiosecuritySectionScores = biosecuritySectionScores;
        Findings = findings;
        PhotoIds = photoIds;
        PriorityPractices = priorityPractices;
    }

    public DateOnly Date { get; }

    public ImmutableArray<RegieAction> RegieActions { get; }

    public ImmutableArray<BiosecurityResponse> BiosecurityResponses { get; }

    public ImmutableArray<BiosecuritySectionScore> BiosecuritySectionScores { get; }

    public ImmutableArray<VisitFinding> Findings { get; }

    public ImmutableArray<Guid> PhotoIds { get; }

    public ImmutableArray<PriorityPractice> PriorityPractices { get; }

    /// <summary>
    /// Creates a stable summary. Biosecurity responses must be supplied in grid order;
    /// No answers are prioritized before Partial answers, and grid order resolves ties.
    /// </summary>
    public static VisitReportSummary Create(
        DateOnly date,
        IEnumerable<RegieAction> regieActions,
        IEnumerable<BiosecurityResponse> biosecurityResponses,
        IEnumerable<VisitFinding> findings,
        IEnumerable<Guid> photoIds)
    {
        ArgumentNullException.ThrowIfNull(regieActions);
        ArgumentNullException.ThrowIfNull(biosecurityResponses);
        ArgumentNullException.ThrowIfNull(findings);
        ArgumentNullException.ThrowIfNull(photoIds);

        var actions = regieActions.ToImmutableArray();
        var responses = biosecurityResponses.ToImmutableArray();
        var visitFindings = findings.ToImmutableArray();
        var photos = photoIds.ToImmutableArray();

        if (actions.Any(action => action is null))
        {
            throw new ArgumentException("Regie actions cannot contain null entries.", nameof(regieActions));
        }

        if (responses.Any(response => response is null))
        {
            throw new ArgumentException("Biosecurity responses cannot contain null entries.", nameof(biosecurityResponses));
        }

        if (visitFindings.Any(finding => finding is null))
        {
            throw new ArgumentException("Findings cannot contain null entries.", nameof(findings));
        }

        return new VisitReportSummary(
            date,
            actions,
            responses,
            CalculateSectionScores(responses),
            visitFindings,
            photos,
            CalculatePriorityPractices(responses));
    }

    private static ImmutableArray<BiosecuritySectionScore> CalculateSectionScores(
        ImmutableArray<BiosecurityResponse> responses)
    {
        var sections = new List<(string Name, decimal EarnedPoints, int AvailablePoints, int ApplicableQuestions)>();

        foreach (var response in responses)
        {
            var sectionIndex = sections.FindIndex(section => section.Name == response.Question.Section);
            if (sectionIndex < 0)
            {
                sections.Add((response.Question.Section, 0m, 0, 0));
                sectionIndex = sections.Count - 1;
            }

            var section = sections[sectionIndex];
            if (response.Answer == Answer.NotApplicable)
            {
                continue;
            }

            var earnedPoints = response.Answer switch
            {
                Answer.Yes => response.Question.Weight,
                Answer.Partial => response.Question.Weight / 2m,
                Answer.No => 0m,
                _ => throw new ArgumentOutOfRangeException(nameof(responses), "The answer is not supported.")
            };

            sections[sectionIndex] = (
                section.Name,
                section.EarnedPoints + earnedPoints,
                section.AvailablePoints + response.Question.Weight,
                section.ApplicableQuestions + 1);
        }

        return sections
            .Select(section => new BiosecuritySectionScore(
                section.Name,
                section.EarnedPoints,
                section.AvailablePoints,
                section.ApplicableQuestions))
            .ToImmutableArray();
    }

    private static ImmutableArray<PriorityPractice> CalculatePriorityPractices(
        ImmutableArray<BiosecurityResponse> responses) =>
        responses
            .Select((response, gridOrder) => (Response: response, GridOrder: gridOrder))
            .Where(item => item.Response.Answer is Answer.No or Answer.Partial)
            .OrderBy(item => item.Response.Answer == Answer.No ? 0 : 1)
            .ThenBy(item => item.GridOrder)
            .Take(3)
            .Select(item => new PriorityPractice(item.Response.Question, item.Response.Answer))
            .ToImmutableArray();
}
