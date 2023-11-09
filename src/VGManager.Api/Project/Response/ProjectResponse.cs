using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.Projects.Responses;

public class ProjectResponse
{
    [Required]
    public string Name { get; set; } = null!;
}
