using System.ComponentModel.DataAnnotations;
using VGManager.Api.VariableGroups.Request;

namespace VGManager.Api.VariableGroup.Request;

public class VariableGroupGetRequest: VariableGroupRequest
{
    [Required]
    public bool KeyIsRegex { get; set; }
}
