using System.ComponentModel.DataAnnotations;
using VGManager.Api.Common;

namespace VGManager.Api.Handlers.Pipelines.BuildPipeline;

public record RunBuildPipelinesRequest : ExtendedBasicRequest
{
    [Required]
    public IEnumerable<IDictionary<string, string>> Pipelines { get; set; } = null!;
}
