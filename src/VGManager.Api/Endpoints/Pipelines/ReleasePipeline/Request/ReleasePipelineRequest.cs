using System.ComponentModel.DataAnnotations;
using VGManager.Api.Common;

namespace VGManager.Api.Endpoints.Pipelines.Release.Request;

public class ReleasePipelineRequest : BasicRequest
{
    [Required]
    public string Project { get; set; } = null!;

    [Required]
    public string RepositoryName { get; set; } = null!;

    [Required]
    public string ConfigFile { get; set; } = null!;
}
