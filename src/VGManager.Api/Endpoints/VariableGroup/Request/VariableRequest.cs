using System.ComponentModel.DataAnnotations;
using VGManager.Api.Common;

namespace VGManager.Api.Endpoints.VariableGroup.Request;

public class VariableRequest : BasicRequest
{
    [Required]
    public string Project { get; set; } = null!;

    [Required]
    public string UserName { get; set; } = null!;

    [Required]
    public string VariableGroupFilter { get; set; } = null!;

    [Required]
    public string KeyFilter { get; set; } = null!;

    [Required]
    public bool ContainsSecrets { get; set; }

    public bool? KeyIsRegex { get; set; }

    public string? ValueFilter { get; set; }
}
