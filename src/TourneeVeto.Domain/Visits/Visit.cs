using System.Collections.Immutable;

namespace TourneeVeto.Domain.Visits;

public sealed record Farm(
    string Id,
    string Name,
    string Municipality,
    int CowCount);

public sealed record Visit(
    Guid Id,
    string FarmId,
    DateOnly Date,
    string Reason,
    string Notes,
    ImmutableArray<Guid> PhotoIds);
