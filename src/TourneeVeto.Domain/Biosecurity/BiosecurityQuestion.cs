namespace TourneeVeto.Domain.Biosecurity;

public sealed record BiosecurityQuestion
{
    public BiosecurityQuestion(
        string Id,
        string Section,
        string Text,
        int Weight,
        bool IsCritical)
    {
        if (Weight is < 1 or > 3)
        {
            throw new ArgumentOutOfRangeException(nameof(Weight), Weight, "Weight must be between 1 and 3.");
        }

        this.Id = Id;
        this.Section = Section;
        this.Text = Text;
        this.Weight = Weight;
        this.IsCritical = IsCritical;
    }

    public string Id { get; }

    public string Section { get; }

    public string Text { get; }

    public int Weight { get; }

    public bool IsCritical { get; }
}

public enum Answer
{
    Yes,
    Partial,
    No,
    NotApplicable
}
