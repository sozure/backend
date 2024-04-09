namespace VGManager.Services.Models;

public class GitPRRequest
{
    public required string Organization { get; set; }
    public required string PAT { get; set; }
    public string? Project { get; set; }
}
