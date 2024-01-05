using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.ReleasePipeline.Request;

public class ProjectsWithCorrespondingReleasePipelineRequest
{
    [Required]
    public string Organization { get; set; } = null!;

    [Required]
    public IEnumerable<string> Projects { get; set; } = null!;

    [Required]
    public string PAT { get; set; } = null!;

    [Required]
    public string RepositoryName { get; set; } = null!;
}
