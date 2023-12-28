
using VGManager.AzureAdapter.Entities;

namespace VGManager.Services.Models.GitRepositories;

public class GitRepositoryResults
{
    public AdapterStatus Status { get; set; }

    public IEnumerable<GitRepositoryResult> Repositories { get; set; } = null!;
}
