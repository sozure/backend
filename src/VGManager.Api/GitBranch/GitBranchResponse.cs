using VGManager.AzureAdapter.Entities;

namespace VGManager.Api.GitBranch;

public class GitBranchResponse
{
    public AdapterStatus Status { get; set; }

    public IEnumerable<string> Branches { get; set; } = null!;
}
