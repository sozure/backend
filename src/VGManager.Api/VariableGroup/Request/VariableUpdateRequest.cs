using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.VariableGroups.Request;

public class VariableUpdateRequest : VariableRequest
{
    [Required]
    public string NewValue { get; set; } = null!;
}
