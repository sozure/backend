using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.VariableGroups.Request.FromSingleProject;

public class VariableGroupUpdateSingleRequest : VariableGroupSingleRequest
{
    [Required]
    public string NewValue { get; set; } = null!;
}
