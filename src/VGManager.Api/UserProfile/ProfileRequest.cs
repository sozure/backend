using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.UserProfile;

public class ProfileRequest
{
    [Required]
    public string Organization { get; set; } = null!;
    [Required]
    public string PAT { get; set; } = null!;
}
