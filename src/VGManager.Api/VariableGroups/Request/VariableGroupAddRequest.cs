using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.VariableGroups.Request;

public class VariableGroupAddRequest : VariableGroupRequest
{
    [Required]
    public string Key { get; set; } = null!;

    [Required]
    public string Value { get; set; } = null!;
}
