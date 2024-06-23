namespace VGManager.Services.Models.GitRepositories;

public record GitRepositoryResult
{
    public string RepositoryId { get; set; } = null!;
    public string RepositoryName { get; set; } = null!;
    public string ProjectName { get; set; } = null!;
}
