using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.ReleasePipeline.Request;

public class ReleasePipelineRequest
{
    [Required]
    public string Organization { get; set; } = null!;

    [Required]
    public string Project { get; set; } = null!;

    [Required]
    public string PAT { get; set; } = null!;

    [Required]
    public string RepositoryName { get; set; } = null!;
}
