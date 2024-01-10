using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.Common;

public class BasicRequest
{
    [Required]
    public string Organization { get; set; } = null!;
    [Required]
    public string PAT { get; set; } = null!;
}
