using Bunit;
using TourneeVeto.Domain;
using TourneeVeto.Domain.Biosecurity;
using TourneeVeto.Domain.Visits;
using TourneeVeto.Ui.Reports;

namespace TourneeVeto.Tests;

public sealed class VisitReportComponentTests
{
    [Fact]
    public async Task Imprimer_declenche_une_seule_invocation_et_expose_un_libelle_accessible()
    {
        var report = VisitReportSummary.Create(
            new DateOnly(2026, 10, 9),
            Array.Empty<RegieAction>(),
            Array.Empty<BiosecurityResponse>(),
            Array.Empty<VisitFinding>(),
            Array.Empty<Guid>());

        using var context = new BunitContext();
        context.JSInterop.SetupVoid("window.print");

        var rendered = context.Render<VisitReport>(parameters => parameters
            .Add(component => component.Report, report));
        var button = rendered.Find("button");

        Assert.Equal("button", button.GetAttribute("type"));
        Assert.Equal("Imprimer / PDF", button.TextContent.Trim());

        await button.ClickAsync();

        var invocation = Assert.Single(context.JSInterop.Invocations);
        Assert.Equal("window.print", invocation.Identifier);
    }

    [Fact]
    public void Rendre_affiche_les_actions_scores_priorites_constats_et_photos()
    {
        var today = new DateOnly(2026, 10, 9);
        var (farms, cows) = DemoData.Generate(today, seed: 42);
        var photoId = Guid.Parse("00000000-0000-0000-0000-000000000101");
        var photoSansImageId = Guid.Parse("00000000-0000-0000-0000-000000000102");
        var responses = new[]
        {
            Reponse("hygiene-partielle", "Hygiène", "Nettoyage fictif à améliorer", 2, Answer.Partial),
            Reponse("hygiene-non-1", "Hygiène", "Protocole fictif absent", 1, Answer.No),
            Reponse("hygiene-non-2", "Hygiène", "Registre fictif incomplet", 1, Answer.No),
            Reponse("hygiene-oui", "Hygiène", "Zone fictive nettoyée", 1, Answer.Yes),
            Reponse("eau-na", "Abreuvement", "Question fictive sans objet", 2, Answer.NotApplicable)
        };
        var report = VisitReportSummary.Create(
            today,
            new[]
            {
                new RegieAction(cows[0].Id, cows[0].Name, RegiePractice.Calving, RegieActionStatus.Planned, "Surveillance fictive"),
                new RegieAction(cows[1].Id, cows[1].Name, RegiePractice.DryOff, RegieActionStatus.Completed)
            },
            responses,
            new[]
            {
                new VisitFinding("Constat fictif : abreuvoir à vérifier."),
                new VisitFinding("Constat fictif : ventilation à ajuster.")
            },
            new[] { photoId, photoSansImageId });
        var photos = new[]
        {
            new VisitReport.PhotoPresentation(
                photoId.ToString(),
                "photos/local/inspection-fictive.jpg",
                "Abreuvoir fictif présentant une trace de calcaire")
        };

        using var context = new BunitContext();
        var rendered = context.Render<VisitReport>(parameters => parameters
            .Add(component => component.Report, report)
            .Add(component => component.FarmName, farms[0].Name)
            .Add(component => component.Photos, photos));

        Assert.Equal(farms[0].Name, rendered.Find("h1").TextContent);
        Assert.Equal("2026-10-09", rendered.Find("time").GetAttribute("datetime"));

        var actions = rendered.FindAll("tbody tr");
        Assert.Equal(2, actions.Count);
        Assert.Contains(cows[0].Name, actions[0].TextContent);
        Assert.Contains("Vêlage", actions[0].TextContent);
        Assert.Contains("Prévue", actions[0].TextContent);
        Assert.Contains("Surveillance fictive", actions[0].TextContent);
        Assert.Contains("Tarissement", actions[1].TextContent);
        Assert.Contains("Terminée", actions[1].TextContent);
        Assert.Contains("—", actions[1].TextContent);
        Assert.NotNull(rendered.Find(".table-scroll"));

        var score = rendered.Find(".score-item");
        Assert.Contains("Hygiène", score.TextContent);
        Assert.Contains("40 %", score.TextContent);
        Assert.Contains("2 / 5 points", score.TextContent);
        Assert.Contains("4 question(s) applicable(s)", score.TextContent);
        Assert.Contains("Aucune question applicable dans cette section.", rendered.Find("article").TextContent);

        var priorities = rendered.FindAll("section[aria-label='Pratiques prioritaires'] .detail-list > li");
        Assert.Equal(3, priorities.Count);
        Assert.Contains("Protocole fictif absent", priorities[0].TextContent);
        Assert.Contains("Hygiène — Non", priorities[0].TextContent);
        Assert.Contains("Registre fictif incomplet", priorities[1].TextContent);
        Assert.Contains("Nettoyage fictif à améliorer", priorities[2].TextContent);
        Assert.Contains("Hygiène — Partielle", priorities[2].TextContent);

        var findings = rendered.FindAll("section[aria-label='Constats'] .detail-list > li");
        Assert.Equal(2, findings.Count);
        Assert.Contains("abreuvoir à vérifier", findings[0].TextContent);
        Assert.Contains("ventilation à ajuster", findings[1].TextContent);

        var photoReferences = rendered.FindAll(".photo-reference");
        Assert.Equal(2, photoReferences.Count);
        var image = photoReferences[0].QuerySelector("img");
        Assert.Equal("photos/local/inspection-fictive.jpg", image?.GetAttribute("src"));
        Assert.Equal("Abreuvoir fictif présentant une trace de calcaire", image?.GetAttribute("alt"));
        Assert.Contains(photoSansImageId.ToString(), photoReferences[1].TextContent);
        Assert.Contains("Aucune image locale disponible pour cette référence.", photoReferences[1].TextContent);

        Assert.NotNull(rendered.Find("article.visit-report"));
        Assert.Equal(5, rendered.FindAll(".report-section").Count);
    }

    [Fact]
    public void Rendre_signale_une_section_sans_question_applicable()
    {
        var report = VisitReportSummary.Create(
            new DateOnly(2026, 10, 9),
            Array.Empty<RegieAction>(),
            new[]
            {
                Reponse("abreuvement-na", "Abreuvement", "Question fictive non applicable", 2, Answer.NotApplicable)
            },
            Array.Empty<VisitFinding>(),
            Array.Empty<Guid>());

        using var context = new BunitContext();
        var rendered = context.Render<VisitReport>(parameters => parameters
            .Add(component => component.Report, report));

        var section = rendered.Find(".score-item");
        Assert.Contains("Abreuvement", section.TextContent);
        Assert.Contains("Aucune question applicable dans cette section.", section.TextContent);
        Assert.Empty(rendered.FindAll(".score-value"));
        Assert.Contains("Aucune pratique prioritaire à signaler.", rendered.Find("article").TextContent);
    }

    [Fact]
    public void Rendre_affiche_les_etats_vides_quand_le_rapport_ne_contient_aucune_donnee()
    {
        var report = VisitReportSummary.Create(
            new DateOnly(2026, 10, 9),
            Array.Empty<RegieAction>(),
            Array.Empty<BiosecurityResponse>(),
            Array.Empty<VisitFinding>(),
            Array.Empty<Guid>());

        using var context = new BunitContext();
        var rendered = context.Render<VisitReport>(parameters => parameters
            .Add(component => component.Report, report));

        var reportElement = rendered.Find("article");
        Assert.Contains("Aucune action de régie à afficher.", reportElement.TextContent);
        Assert.Contains("Aucun score de biosécurité à afficher.", reportElement.TextContent);
        Assert.Contains("Aucune pratique prioritaire à signaler.", reportElement.TextContent);
        Assert.Contains("Aucun constat consigné.", reportElement.TextContent);
        Assert.Contains("Aucune référence photo associée à cette visite.", reportElement.TextContent);
        Assert.Equal(5, rendered.FindAll(".empty-state").Count);
    }

    [Theory]
    [InlineData("https://exemple-fictif.test/photo.jpg")]
    [InlineData("javascript:alert(1)")]
    [InlineData(" javascript:alert(1)")]
    [InlineData("JavaScript:alert(1)")]
    [InlineData("java\tscript:alert(1)")]
    [InlineData("data:image/png;base64,AAAA")]
    [InlineData("//serveur-fictif/partage/photo.jpg")]
    [InlineData(" //serveur-fictif/x.jpg")]
    [InlineData(@"\\serveur-fictif\partage\photo.jpg")]
    [InlineData("/\\serveur-fictif/x.jpg")]
    [InlineData("blob:x")]
    [InlineData("")]
    [InlineData("/photos/a.jpg")]
    [InlineData("photos/inspection fictive.jpg")]
    public void Rendre_refuse_une_source_photo_non_locale_et_affiche_un_etat_explicite(string localSource)
    {
        var photoId = Guid.Parse("00000000-0000-0000-0000-000000000201");
        var report = VisitReportSummary.Create(
            new DateOnly(2026, 10, 9),
            Array.Empty<RegieAction>(),
            Array.Empty<BiosecurityResponse>(),
            Array.Empty<VisitFinding>(),
            new[] { photoId });

        using var context = new BunitContext();
        var rendered = context.Render<VisitReport>(parameters => parameters
            .Add(component => component.Report, report)
            .Add(component => component.Photos, new[]
            {
                new VisitReport.PhotoPresentation(
                    photoId.ToString(),
                    localSource,
                    "Photo fictive")
            }));

        Assert.Empty(rendered.FindAll("img"));
        Assert.Contains(
            "La source de cette image est invalide; aucune image n’est affichée.",
            rendered.Find(".photo-reference").TextContent);
    }

    [Theory]
    [InlineData("photos/a.jpg")]
    [InlineData("./photos/a.jpg")]
    public void Rendre_affiche_une_source_photo_relative_locale(string localSource)
    {
        var photoId = Guid.Parse("00000000-0000-0000-0000-000000000202");
        var report = VisitReportSummary.Create(
            new DateOnly(2026, 10, 9),
            Array.Empty<RegieAction>(),
            Array.Empty<BiosecurityResponse>(),
            Array.Empty<VisitFinding>(),
            new[] { photoId });

        using var context = new BunitContext();
        var rendered = context.Render<VisitReport>(parameters => parameters
            .Add(component => component.Report, report)
            .Add(component => component.Photos, new[]
            {
                new VisitReport.PhotoPresentation(
                    photoId.ToString(),
                    localSource,
                    "Photo fictive")
            }));

        var image = Assert.Single(rendered.FindAll("img"));
        Assert.Equal(localSource, image.GetAttribute("src"));
        Assert.Equal("Photo fictive", image.GetAttribute("alt"));
    }

    [Fact]
    public void Rendre_le_nom_de_ferme_et_le_texte_alternatif_comme_texte_sans_injecter_de_balise()
    {
        const string texteMalveillant = "<img src=x onerror=alert(1)>";
        var photoId = Guid.Parse("00000000-0000-0000-0000-000000000203");
        var report = VisitReportSummary.Create(
            new DateOnly(2026, 10, 9),
            Array.Empty<RegieAction>(),
            Array.Empty<BiosecurityResponse>(),
            Array.Empty<VisitFinding>(),
            new[] { photoId });

        using var context = new BunitContext();
        var rendered = context.Render<VisitReport>(parameters => parameters
            .Add(component => component.Report, report)
            .Add(component => component.FarmName, texteMalveillant)
            .Add(component => component.Photos, new[]
            {
                new VisitReport.PhotoPresentation(
                    photoId.ToString(),
                    "photos/a.jpg",
                    texteMalveillant)
            }));

        Assert.Equal(texteMalveillant, rendered.Find("h1").TextContent);
        var image = Assert.Single(rendered.FindAll("img"));
        Assert.Equal("photos/a.jpg", image.GetAttribute("src"));
        Assert.Equal(texteMalveillant, image.GetAttribute("alt"));
        Assert.Empty(rendered.FindAll("h1 img"));
        Assert.Empty(rendered.FindAll("[onerror]"));
    }

    [Fact]
    public void Rendre_conserve_les_balises_des_notes_et_constats_comme_texte()
    {
        var today = new DateOnly(2026, 10, 9);
        var (_, cows) = DemoData.Generate(today, seed: 42);
        const string note = "<img src=x onerror=alert(1)><strong>Note fictive</strong>";
        const string finding = "<script>alert(1)</script><em>Constat fictif</em>";
        var report = VisitReportSummary.Create(
            today,
            new[]
            {
                new RegieAction(
                    cows[0].Id,
                    cows[0].Name,
                    RegiePractice.Calving,
                    RegieActionStatus.Planned,
                    note)
            },
            Array.Empty<BiosecurityResponse>(),
            new[] { new VisitFinding(finding) },
            Array.Empty<Guid>());

        using var context = new BunitContext();
        var rendered = context.Render<VisitReport>(parameters => parameters
            .Add(component => component.Report, report));

        Assert.Equal(note, rendered.Find("td[data-label='Note']").TextContent);
        Assert.Equal(finding, rendered.Find("section[aria-label='Constats'] .detail-list > li").TextContent);
        Assert.Empty(rendered.FindAll("img, strong, script, em"));
    }

    private static BiosecurityResponse Reponse(
        string id,
        string section,
        string text,
        int weight,
        Answer answer) =>
        new(new BiosecurityQuestion(id, section, text, weight, IsCritical: false), answer);
}
