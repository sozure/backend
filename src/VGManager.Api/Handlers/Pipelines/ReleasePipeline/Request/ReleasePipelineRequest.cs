using System.ComponentModel.DataAnnotations;
using VGManager.Api.Common;

namespace VGManager.Api.Handlers.Pipelines.Release.Request;

public record ReleasePipelineRequest : ExtendedBasicRequest
{
    [Required]
    public string RepositoryName { get; set; } = null!;

    [Required]
    public string ConfigFile { get; set; } = null!;
}
