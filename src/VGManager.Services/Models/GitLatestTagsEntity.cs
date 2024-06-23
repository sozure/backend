namespace VGManager.Services.Models;

public record GitLatestTagsEntity
{
    public string Organization { get; set; } = null!;
    public string PAT { get; set; } = null!;
    public required Guid[] RepositoryIds { get; set; }
}
