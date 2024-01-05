using VGManager.AzureAdapter.Entities;

namespace VGManager.Api.GitFile;

public class GitConfigFileResponse
{
    public AdapterStatus Status { get; set; }
    public IEnumerable<string> ConfigFiles { get; set; } = null!;
}
