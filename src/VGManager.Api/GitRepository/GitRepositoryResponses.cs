using VGManager.AzureAdapter.Entities;

namespace VGManager.Api.GitRepository;

public class GitRepositoryResponses
{
    public AdapterStatus Status { get; set; }

    public IEnumerable<GitRepositoryResponse> Repositories { get; set; } = null!;
}
