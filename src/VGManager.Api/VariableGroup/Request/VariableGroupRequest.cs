using System.ComponentModel.DataAnnotations;
using VGManager.Api.VariableGroups.Request;

namespace VGManager.Api.VariableGroup.Request;

public class VariableGroupRequest: VariableRequest
{
    [Required]
    public bool ContainsKey { get; set; }
}
