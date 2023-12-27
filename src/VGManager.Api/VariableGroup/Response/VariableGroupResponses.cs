using System.ComponentModel.DataAnnotations;
using VGManager.AzureAdapter.Entities;

namespace VGManager.Api.VariableGroup.Response;

public class VariableGroupResponses
{
    [Required]
    public AdapterStatus Status { get; set; }

    [Required]
    public List<VariableGroupResponse> VariableGroups { get; set; } = null!;
}
