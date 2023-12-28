using VGManager.AzureAdapter.Entities;

namespace VGManager.Api.GitRepository;

public class GitRepositoryResponse
{
    public AdapterStatus Status { get; set; }

    public IEnumerable<string> Repositories { get; set; } = null!;
}
