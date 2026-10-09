using TourneeVeto.Domain.Biosecurity;
using TourneeVeto.Domain.Visits;

namespace TourneeVeto.Tests;

public sealed class VisitReportSummaryTests
{
    [Fact]
    public void Creer_calcule_les_scores_ponderes_par_section_en_excluant_les_non_applicables()
    {
        var responses = new[]
        {
            Reponse("hygiene-oui", "Hygiène", weight: 3, Answer.Yes),
            Reponse("hygiene-partielle", "Hygiène", weight: 2, Answer.Partial),
            Reponse("hygiene-na", "Hygiène", weight: 1, Answer.NotApplicable),
            Reponse("logement-non", "Logement", weight: 2, Answer.No),
            Reponse("logement-oui", "Logement", weight: 1, Answer.Yes),
            Reponse("traite-na", "Traite", weight: 2, Answer.NotApplicable)
        };

        var summary = CreerResume(biosecurityResponses: responses);

        Assert.Collection(
            summary.BiosecuritySectionScores,
            score =>
            {
                Assert.Equal("Hygiène", score.Section);
                Assert.Equal(4m, score.EarnedPoints);
                Assert.Equal(5, score.AvailablePoints);
                Assert.Equal(2, score.ApplicableQuestionCount);
                Assert.Equal(80m, score.ScorePercent);
            },
            score =>
            {
                Assert.Equal("Logement", score.Section);
                Assert.Equal(1m, score.EarnedPoints);
                Assert.Equal(3, score.AvailablePoints);
                Assert.Equal(2, score.ApplicableQuestionCount);
                Assert.Equal(33.33m, score.ScorePercent);
            },
            score =>
            {
                Assert.Equal("Traite", score.Section);
                Assert.Equal(0m, score.EarnedPoints);
                Assert.Equal(0, score.AvailablePoints);
                Assert.Equal(0, score.ApplicableQuestionCount);
                Assert.Null(score.ScorePercent);
            });
    }

    [Fact]
    public void Creer_classe_les_non_avant_les_partiels_et_conserve_l_ordre_de_grille_a_egalite()
    {
        var responses = new[]
        {
            Reponse("partiel-premier", "Hygiène", weight: 1, Answer.Partial),
            Reponse("non-premier", "Hygiène", weight: 1, Answer.No),
            Reponse("partiel-second", "Hygiène", weight: 1, Answer.Partial),
            Reponse("non-second", "Hygiène", weight: 1, Answer.No),
            Reponse("partiel-troisieme", "Hygiène", weight: 1, Answer.Partial)
        };

        var summary = CreerResume(biosecurityResponses: responses);

        Assert.Equal(
            new[] { "non-premier", "non-second", "partiel-premier" },
            summary.PriorityPractices.Select(practice => practice.Question.Id));
        Assert.Equal(
            new[] { Answer.No, Answer.No, Answer.Partial },
            summary.PriorityPractices.Select(practice => practice.Answer));
    }

    [Fact]
    public void Creer_agrege_les_actions_constats_et_references_photo_dans_leur_ordre()
    {
        var actions = new[]
        {
            new RegieAction("animal-fictif-1", "Vache fictive 1", RegiePractice.Calving, RegieActionStatus.Planned, "Action fictive"),
            new RegieAction("animal-fictif-2", "Vache fictive 2", RegiePractice.DryOff, RegieActionStatus.Completed)
        };
        var findings = new[]
        {
            new VisitFinding("Constat fictif 1"),
            new VisitFinding("Constat fictif 2")
        };
        var photos = new[]
        {
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Guid.Parse("00000000-0000-0000-0000-000000000002")
        };

        var summary = CreerResume(
            regieActions: actions,
            findings: findings,
            photoIds: photos);

        Assert.Equal(actions, summary.RegieActions);
        Assert.Equal(findings, summary.Findings);
        Assert.Equal(photos, summary.PhotoIds);
        Assert.Equal(new DateOnly(2026, 10, 9), summary.Date);
    }

    private static VisitReportSummary CreerResume(
        IEnumerable<RegieAction>? regieActions = null,
        IEnumerable<BiosecurityResponse>? biosecurityResponses = null,
        IEnumerable<VisitFinding>? findings = null,
        IEnumerable<Guid>? photoIds = null) =>
        VisitReportSummary.Create(
            new DateOnly(2026, 10, 9),
            regieActions ?? Array.Empty<RegieAction>(),
            biosecurityResponses ?? Array.Empty<BiosecurityResponse>(),
            findings ?? Array.Empty<VisitFinding>(),
            photoIds ?? Array.Empty<Guid>());

    private static BiosecurityResponse Reponse(
        string id,
        string section,
        int weight,
        Answer answer) =>
        new(
            new BiosecurityQuestion(id, section, $"Question fictive {id}", weight, IsCritical: false),
            answer);
}
