using VGManager.AzureAdapter.Entities;
using VGManager.Models;

namespace VGManager.Api.ReleasePipeline.Response;

public class ProjectsWithCorrespondingReleasePipelineResponse
{
    public AdapterStatus Status { get; set; }

    public IEnumerable<string> Projects { get; set; } = null!;
}
