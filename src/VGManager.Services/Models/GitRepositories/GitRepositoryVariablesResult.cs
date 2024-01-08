using VGManager.Models;

namespace VGManager.Services.Models.GitRepositories;

public class GitRepositoryVariablesResult
{
    public AdapterStatus Status { get; set; }

    public IEnumerable<string> Variables { get; set; } = null!;
}
