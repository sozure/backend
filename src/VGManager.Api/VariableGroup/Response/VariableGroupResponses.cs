using System.ComponentModel.DataAnnotations;
using VGManager.Models;

namespace VGManager.Api.VariableGroup.Response;

public class VariableGroupResponses
{
    [Required]
    public AdapterStatus Status { get; set; }

    [Required]
    public List<VariableGroupResponse> VariableGroups { get; set; } = null!;
}
