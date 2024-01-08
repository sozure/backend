using VGManager.AzureAdapter.Entities;
using VGManager.Models;

namespace VGManager.Api.GitRepository.Response;

public class GitRepositoryVariablesResponse
{
    public AdapterStatus Status { get; set; }

    public IEnumerable<string> Variables { get; set; } = null!;
}
