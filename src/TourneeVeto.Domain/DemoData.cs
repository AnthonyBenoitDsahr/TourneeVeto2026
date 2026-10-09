using TourneeVeto.Domain.Herd;
using TourneeVeto.Domain.Visits;

namespace TourneeVeto.Domain;

public static class DemoData
{
    private const int CowCount = 60;

    public static (IReadOnlyList<Farm> Farms, IReadOnlyList<Cow> Cows) Generate(DateOnly today, int seed)
    {
        var random = new Random(seed);
        var farms = Enumerable.Range(1, 3)
            .Select(index => new Farm(
                Id: $"farm-{index:00}",
                Name: $"Élevage fictif {index:00}",
                Municipality: $"Municipalité fictive {index:00}",
                CowCount: CowCount / 3))
            .ToArray();

        var cows = new Cow[CowCount];
        for (var index = 0; index < cows.Length; index++)
        {
            var isHeifer = index < 5;
            var isDry = index is 5 or 6;
            var isCalvingSoon = index is 7 or 8;
            var needsPregnancyDiagnosis = index is >= 9 and <= 11;
            var hasHighScc = index is >= 12 and <= 15;
            var lactation = isHeifer ? 0 : random.Next(1, 6);

            var status = isDry
                ? ReproStatus.Dry
                : isCalvingSoon
                    ? ReproStatus.Pregnant
                    : needsPregnancyDiagnosis
                        ? ReproStatus.Bred
                        : isHeifer || random.Next(2) == 0
                            ? ReproStatus.Open
                            : ReproStatus.Pregnant;

            DateOnly? lastInsemination = status switch
            {
                ReproStatus.Bred => today.AddDays(-random.Next(30, 46)),
                ReproStatus.Pregnant when isCalvingSoon => today.AddDays(-random.Next(269, 284)),
                ReproStatus.Pregnant => today.AddDays(-random.Next(46, 251)),
                ReproStatus.Dry => today.AddDays(-random.Next(120, 251)),
                _ => null
            };

            DateOnly? lastCalving = lactation > 0 && random.Next(4) != 0
                ? lastInsemination?.AddDays(-random.Next(60, 301)) ?? today.AddDays(-random.Next(30, 301))
                : null;
            var bornOn = today.AddDays(-random.Next(isHeifer ? 365 : 730, isHeifer ? 731 : 3651));
            var scc = random.Next(201);
            var lastSccThousands = hasHighScc
                ? random.Next(201, 601)
                : scc == 0
                    ? (int?)null
                    : scc;

            cows[index] = new Cow(
                Id: $"cow-{index + 1:000}",
                Name: $"Vache fictive {index + 1:000}",
                BornOn: bornOn,
                Lactation: lactation,
                LastCalving: lastCalving,
                LastInsemination: lastInsemination,
                Status: status,
                LastSccThousands: lastSccThousands);
        }

        return (Array.AsReadOnly(farms), Array.AsReadOnly(cows));
    }
}
