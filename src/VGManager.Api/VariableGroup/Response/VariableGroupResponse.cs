using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.VariableGroup.Response;

public class VariableGroupResponse
{
    [Required]
    public string Project { get; set; } = null!;

    [Required]
    public string VariableGroupName { get; set; } = null!;

    [Required]
    public string VariableGroupType { get; set; } = null!;
}
