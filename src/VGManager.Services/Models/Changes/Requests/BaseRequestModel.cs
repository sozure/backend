namespace VGManager.Services.Models.Changes.Requests;

public abstract class BaseRequestModel
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public int Limit { get; set; }
    public string? User { get; set; } = null!;
}
