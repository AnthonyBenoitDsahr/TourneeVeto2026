namespace TourneeVeto.Domain.Biosecurity;

public sealed record BiosecurityResponse
{
    public BiosecurityResponse(BiosecurityQuestion question, Answer answer, string? note = null)
    {
        ArgumentNullException.ThrowIfNull(question);

        if (!Enum.IsDefined(answer))
        {
            throw new ArgumentOutOfRangeException(nameof(answer), answer, "The answer is not supported.");
        }

        Question = question;
        Answer = answer;
        Note = note;
    }

    public BiosecurityQuestion Question { get; }

    public Answer Answer { get; }

    public string? Note { get; }
}

/// <summary>
/// The weighted, simplified compliance result for one biosecurity section.
/// The percentage is unavailable when all questions in the section are not applicable.
/// Yes earns the full question weight, Partial earns half, No earns zero, and
/// NotApplicable is excluded from the available points.
/// </summary>
public sealed record BiosecuritySectionScore
{
    public BiosecuritySectionScore(
        string section,
        decimal earnedPoints,
        int availablePoints,
        int applicableQuestionCount)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(section);

        if (availablePoints < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(availablePoints));
        }

        if (earnedPoints < 0 || earnedPoints > availablePoints)
        {
            throw new ArgumentOutOfRangeException(nameof(earnedPoints));
        }

        if (applicableQuestionCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(applicableQuestionCount));
        }

        Section = section;
        EarnedPoints = earnedPoints;
        AvailablePoints = availablePoints;
        ApplicableQuestionCount = applicableQuestionCount;
        ScorePercent = availablePoints == 0
            ? null
            : decimal.Round(earnedPoints * 100m / availablePoints, 2, MidpointRounding.AwayFromZero);
    }

    public string Section { get; }

    public decimal EarnedPoints { get; }

    public int AvailablePoints { get; }

    public int ApplicableQuestionCount { get; }

    public decimal? ScorePercent { get; }
}
