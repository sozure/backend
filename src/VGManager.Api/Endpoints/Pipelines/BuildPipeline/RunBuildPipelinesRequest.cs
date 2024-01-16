using System.ComponentModel.DataAnnotations;
using VGManager.Api.Common;

namespace VGManager.Api.Endpoints.Pipelines.BuildPipeline;

public class RunBuildPipelinesRequest: ExtendedBasicRequest
{
    [Required]
    public IEnumerable<IDictionary<string, string>> Pipelines { get; set; } = null!;
}
