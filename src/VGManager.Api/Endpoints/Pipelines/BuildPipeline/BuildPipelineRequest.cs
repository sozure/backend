using System.ComponentModel.DataAnnotations;
using VGManager.Api.Common;

namespace VGManager.Api.Endpoints.Pipelines.BuildPipeline;

public class BuildPipelineRequest : ExtendedBasicRequest
{
    [Required]
    public int DefinitionId { get; set; }
}
