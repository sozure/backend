using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.Projects;

public class ProjectRequest
{
    [Required]
    public string Organization { get; set; } = null!;

    [Required]
    public string PAT { get; set; } = null!;
}
