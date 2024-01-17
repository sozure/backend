using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.Endpoints.VariableGroup.Request;

public class VariableUpdateRequest : VariableRequest
{
    [Required]
    public string NewValue { get; set; } = null!;
}
