using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.VariableGroups.Request;

public class VariableGroupUpdateRequest : VariableGroupRequest
{
    [Required]
    public string NewValue { get; set; } = null!;

    public string? ValueFilter { get; set; }
}
