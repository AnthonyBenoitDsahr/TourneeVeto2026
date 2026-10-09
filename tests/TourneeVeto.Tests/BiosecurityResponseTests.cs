using TourneeVeto.Domain.Biosecurity;

namespace TourneeVeto.Tests;

public sealed class BiosecurityResponseTests
{
    [Fact]
    public void Une_reponse_conserve_la_question_le_resultat_et_la_note()
    {
        var question = CreerQuestion("question-test", "Hygiène");

        var response = new BiosecurityResponse(question, Answer.Partial, "Exemple fictif");

        Assert.Same(question, response.Question);
        Assert.Equal(Answer.Partial, response.Answer);
        Assert.Equal("Exemple fictif", response.Note);
    }

    [Fact]
    public void Une_reponse_refuse_une_question_nulle()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new BiosecurityResponse(null!, Answer.Yes));
    }

    [Fact]
    public void Une_reponse_refuse_un_resultat_non_pris_en_charge()
    {
        var question = CreerQuestion("question-test", "Hygiène");

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new BiosecurityResponse(question, (Answer)999));
    }

    private static BiosecurityQuestion CreerQuestion(string id, string section) =>
        new(id, section, "Question fictive", Weight: 1, IsCritical: false);
}
