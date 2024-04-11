namespace VGManager.Services.Models;

public class CreatePRsRequest : GitPRRequest
{
    public required string[] Repositories { get; set; }
    public required string SourceBranch { get; set; }
    public required string TargetBranch { get; set; }
    public required string Title { get; set; }
}