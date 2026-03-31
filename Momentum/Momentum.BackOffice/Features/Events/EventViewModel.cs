namespace Momentum.BackOffice.Features.Events;

public class EventViewModel
{
    public Guid Id { get; set; }
    public int Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
}

public sealed class EventEntryViewModel
{
    public int Type { get; set; }
    public string? Name { get; set; }
    public string? Date { get; set; }
}
