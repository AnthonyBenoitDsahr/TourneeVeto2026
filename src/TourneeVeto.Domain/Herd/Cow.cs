namespace TourneeVeto.Domain.Herd;

public enum ReproStatus
{
    Open,
    Bred,
    Pregnant,
    Dry
}

public sealed record Cow(
    string Id,
    string Name,
    DateOnly BornOn,
    int Lactation,
    DateOnly? LastCalving,
    DateOnly? LastInsemination,
    ReproStatus Status,
    int? LastSccThousands);
