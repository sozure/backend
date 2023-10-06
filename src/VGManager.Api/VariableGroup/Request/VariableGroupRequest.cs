using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.VariableGroups.Request;

public class VariableGroupRequest
{
    [Required]
    public string Organization { get; set; } = null!;

    [Required]
    public string Project { get; set; } = null!;

    [Required]
    public string PAT { get; set; } = null!;

    [Required]
    public string VariableGroupFilter { get; set; } = null!;

    [Required]
    public string KeyFilter { get; set; } = null!;

    [Required]
    public bool ContainsSecrets { get; set; }

    [Required]
    public bool? KeyIsRegex { get; set; }

    public string? ValueFilter { get; set; }
}
