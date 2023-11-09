using System.ComponentModel.DataAnnotations;
using VGManager.AzureAdapter.Entities;

namespace VGManager.Api.Projects.Responses;

public class ProjectsResponse
{
    [Required]
    public Status Status { get; set; }

    [Required]
    public IEnumerable<ProjectResponse> Projects { get; set; } = null!;
}
