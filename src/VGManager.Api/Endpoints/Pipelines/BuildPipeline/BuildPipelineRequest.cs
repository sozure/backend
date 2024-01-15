using System.ComponentModel.DataAnnotations;
using VGManager.Api.Common;

namespace VGManager.Api.Endpoints.Pipelines.BuildPipeline;

public class BuildPipelineRequest : BasicRequest
{
    [Required]
    public string Project { get; set; } = null!;

    [Required]
    public int DefinitionId { get; set; }

    [Required]
    public string SourceBranch { get; set; } = null!;
}
