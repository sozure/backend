using System.ComponentModel.DataAnnotations;
using VGManager.Api.Common;

namespace VGManager.Api.Handlers.Pipelines.BuildPipeline;

public record BuildPipelineRequest : ExtendedBasicRequest
{
    [Required]
    public int DefinitionId { get; set; }
}
