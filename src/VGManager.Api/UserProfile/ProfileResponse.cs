using Microsoft.VisualStudio.Services.Profile;
using System.ComponentModel.DataAnnotations;
using VGManager.Models;

namespace VGManager.Api.UserProfile;


public class ProfileResponse
{
    [Required]
    public AdapterStatus Status { get; set; }
    [Required]
    public Profile Profile { get; set; } = null!;
}
