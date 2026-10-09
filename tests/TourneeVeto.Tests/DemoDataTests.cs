using TourneeVeto.Domain;

namespace TourneeVeto.Tests;

public sealed class DemoDataTests
{
    [Fact]
    public void Generer_fournit_au_moins_deux_elevages_et_cinq_animaux_fictifs()
    {
        var today = new DateOnly(2026, 10, 9);

        var (farms, cows) = DemoData.Generate(today, seed: 42);

        Assert.True(farms.Count >= 2);
        Assert.True(cows.Count >= 5);
        Assert.All(farms, farm =>
        {
            Assert.Contains("fictif", farm.Name, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("fictive", farm.Municipality, StringComparison.OrdinalIgnoreCase);
        });
        Assert.All(cows, cow =>
            Assert.Contains("fictive", cow.Name, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Generer_declare_un_nombre_d_animaux_coherent_avec_le_jeu_fourni()
    {
        var today = new DateOnly(2026, 10, 9);

        var (farms, cows) = DemoData.Generate(today, seed: 42);

        Assert.Equal(cows.Count, farms.Sum(farm => farm.CowCount));
    }
}
