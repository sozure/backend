using VGManager.AzureAdapter.Entities;

namespace VGManager.Api.ReleasePipeline;

public class ProjectsWithCorrespondingReleasePipelineResponse
{
    public AdapterStatus Status { get; set; }

    public IEnumerable<string> Projects { get; set; } = null!;
}
