using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.Endpoints.VariableGroup.Request;

public class VariableGroupRequest : VariableRequest
{
    [Required]
    public bool ContainsKey { get; set; }
}
