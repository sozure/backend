using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.VariableGroups.Request.FromSingleProject;

public class VariableGroupSingleRequest: VariableGroupRequest
{
    [Required]
    public string Project { get; set; } = null!;
}
