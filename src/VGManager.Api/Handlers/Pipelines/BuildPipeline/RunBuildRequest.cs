using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.Handlers.Pipelines.BuildPipeline;

public record RunBuildRequest : BuildPipelineRequest
{
    [Required]
    public string SourceBranch { get; set; } = null!;
}
