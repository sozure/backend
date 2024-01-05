using VGManager.Api.GitRepository.Response;
using VGManager.AzureAdapter.Entities;

namespace VGManager.Api.ReleasePipeline;

public class ReleasePipelineVGResponse
{
    public AdapterStatus Status { get; set; }

    public IEnumerable<string> VariableGroups { get; set; } = null!;
}