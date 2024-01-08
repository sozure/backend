using System.ComponentModel.DataAnnotations;
using VGManager.Models;

namespace VGManager.Api.Secrets.Response;

public class SecretResponses
{
    [Required]
    public AdapterStatus Status { get; set; }

    [Required]
    public IEnumerable<SecretResponse> Secrets { get; set; } = null!;
}
