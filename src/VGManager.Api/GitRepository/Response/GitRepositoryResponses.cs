using VGManager.AzureAdapter.Entities;
using VGManager.Models;

namespace VGManager.Api.GitRepository.Response;

public class GitRepositoryResponses
{
    public AdapterStatus Status { get; set; }

    public IEnumerable<GitRepositoryResponse> Repositories { get; set; } = null!;
}
