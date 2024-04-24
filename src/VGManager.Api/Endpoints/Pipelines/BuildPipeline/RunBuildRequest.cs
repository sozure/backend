using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.Endpoints.Pipelines.BuildPipeline;

public class RunBuildRequest : BuildPipelineRequest
{
    [Required]
    public string SourceBranch { get; set; } = null!;
}
