using VGManager.Models;

namespace VGManager.Api.ReleasePipeline.Response;

public class ReleasePipelineVGResponse
{
    public AdapterStatus Status { get; set; }

    public IEnumerable<Dictionary<string, string>> VariableGroups { get; set; } = null!;
}
