using System.ComponentModel.DataAnnotations;
using VGManager.AzureAdapter.Entities;

namespace VGManager.Api.Secrets.Response;

public class SecretResponses
{
    [Required]
    public Status Status { get; set; }

    [Required]
    public IEnumerable<SecretResponse> Secrets { get; set; } = null!;
}
