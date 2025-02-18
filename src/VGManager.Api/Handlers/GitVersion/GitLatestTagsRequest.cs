namespace VGManager.Api.Handlers.GitVersion;

public record GitLatestTagsRequest
{
    public string Organization { get; set; } = null!;
    public string PAT { get; set; } = null!;
    public required Guid[] RepositoryIds { get; set; }
}
