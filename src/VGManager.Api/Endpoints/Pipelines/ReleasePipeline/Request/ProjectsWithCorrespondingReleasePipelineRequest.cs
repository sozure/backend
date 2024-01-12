using System.ComponentModel.DataAnnotations;
using VGManager.Api.Common;

namespace VGManager.Api.Endpoints.Pipelines.Release.Request;

public class ProjectsWithCorrespondingReleasePipelineRequest : BasicRequest
{
    
    [Required]
    public IEnumerable<string> Projects { get; set; } = null!;

    [Required]
    public string RepositoryName { get; set; } = null!;

    [Required]
    public string ConfigFile { get; set; } = null!;
}
