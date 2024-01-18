using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.Common;

public class ExtendedBasicRequest : BasicRequest
{
    [Required]
    public string Project { get; set; } = null!;
}
