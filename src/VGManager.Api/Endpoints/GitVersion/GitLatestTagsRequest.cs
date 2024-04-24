namespace VGManager.Api.Endpoints.GitVersion;

public class GitLatestTagsRequest
{
    public string Organization { get; set; } = null!;
    public string PAT { get; set; } = null!;
    public required Guid[] RepositoryIds { get; set; }
}
