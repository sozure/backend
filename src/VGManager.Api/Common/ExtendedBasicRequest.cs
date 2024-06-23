using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.Common;

public record ExtendedBasicRequest : BasicRequest
{
    [Required]
    public string Project { get; set; } = null!;
}
