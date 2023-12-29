using VGManager.AzureAdapter.Entities;

namespace VGManager.Api.GitRepository;

public class GitRepositoryVariablesResponse
{
    public AdapterStatus Status { get; set; }

    public IEnumerable<string> Variables { get; set; } = null!;
}
