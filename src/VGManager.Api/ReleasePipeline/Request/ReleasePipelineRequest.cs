using System.ComponentModel.DataAnnotations;
using VGManager.Api.Common;

namespace VGManager.Api.ReleasePipeline.Request;

public class ReleasePipelineRequest : BasicRequest
{
    [Required]
    public string Project { get; set; } = null!;

    [Required]
    public string RepositoryName { get; set; } = null!;
}
