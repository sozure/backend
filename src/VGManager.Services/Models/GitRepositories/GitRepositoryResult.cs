
using VGManager.AzureAdapter.Entities;

namespace VGManager.Services.Models.GitRepositories;

public class GitRepositoryResult
{
    public AdapterStatus Status { get; set; }

    public IEnumerable<string> Repositories { get; set; } = null!;
}
