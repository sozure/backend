using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.VariableGroups.Request.FromSingleProject;

public class VariableGroupAddSingleRequest : VariableGroupSingleRequest
{
    [Required]
    public string Key { get; set; } = null!;

    [Required]
    public string Value { get; set; } = null!;
}
