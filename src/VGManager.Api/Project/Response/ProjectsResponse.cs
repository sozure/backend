using System.ComponentModel.DataAnnotations;
using VGManager.Models;

namespace VGManager.Api.Projects.Responses;

public class ProjectsResponse
{
    [Required]
    public AdapterStatus Status { get; set; }

    [Required]
    public IEnumerable<ProjectResponse> Projects { get; set; } = null!;
}
