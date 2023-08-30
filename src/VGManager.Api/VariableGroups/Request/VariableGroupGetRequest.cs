using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.VariableGroups.Request;

public class VariableGroupGetRequest : VariableGroupRequest
{
    public string? ValueFilter { get; set; }
}
