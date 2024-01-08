using System.ComponentModel.DataAnnotations;
using VGManager.Models;

namespace VGManager.Api.VariableGroups.Response;

public class VariableResponses
{
    [Required]
    public AdapterStatus Status { get; set; }

    [Required]
    public List<VariableResponse> Variables { get; set; } = null!;
}
