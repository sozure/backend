using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.VariableGroups.Request.FromMultipleProjects;

public class VariableGroupMultipleRequest: VariableGroupRequest
{
    [Required]
    public string[] Projects { get; set; } = null!;
}
