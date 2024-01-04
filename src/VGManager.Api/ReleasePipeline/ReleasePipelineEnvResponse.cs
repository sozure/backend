using VGManager.Api.GitRepository.Response;
using VGManager.AzureAdapter.Entities;

namespace VGManager.Api.ReleasePipeline;

public class ReleasePipelineEnvResponse
{
    public AdapterStatus Status { get; set; }

    public IEnumerable<string> Environments { get; set; } = null!;
}
