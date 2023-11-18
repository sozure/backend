namespace VGManager.Services.Models.Changes;

public class RequestModel
{
    public string Organization { get; set; } = null!;
    public string Project { get; set; } = null!;
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public int Limit { get; set; }
    public IEnumerable<ChangeType> ChangeTypes { get; set; } = Array.Empty<ChangeType>();
    public string? User { get; set; } = null!;
}
