using VGManager.Models;

namespace VGManager.Api.ReleasePipeline.Response;

public class ReleasePipelineEnvResponse
{
    public AdapterStatus Status { get; set; }

    public IEnumerable<string> Environments { get; set; } = null!;
}
