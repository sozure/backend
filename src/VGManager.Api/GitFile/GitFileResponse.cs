using VGManager.AzureAdapter.Entities;

namespace VGManager.Api.GitFile;

public class GitFileResponse
{
    public AdapterStatus Status { get; set; }
    public IEnumerable<string> FilePaths { get; set; } = null!;
}
