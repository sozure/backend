using VGManager.Api.GitRepository.Response;
using VGManager.AzureAdapter.Entities;

namespace VGManager.Api.ReleasePipeline.Response;

public class ReleasePipelineVGResponse
{
    public AdapterStatus Status { get; set; }

    public IEnumerable<Dictionary<string, string>> VariableGroups { get; set; } = null!;
}