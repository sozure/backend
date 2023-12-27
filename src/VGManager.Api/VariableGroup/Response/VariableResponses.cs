using System.ComponentModel.DataAnnotations;
using VGManager.AzureAdapter.Entities;

namespace VGManager.Api.VariableGroups.Response;

public class VariableResponses
{
    [Required]
    public AdapterStatus Status { get; set; }

    [Required]
    public List<VariableResponse> Variables { get; set; } = null!;
}
