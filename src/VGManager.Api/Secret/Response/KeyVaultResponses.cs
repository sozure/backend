using System.ComponentModel.DataAnnotations;
using VGManager.Models;

namespace VGManager.Api.Secret.Response;

public class KeyVaultResponses
{
    [Required]
    public AdapterStatus Status { get; set; }

    [Required]
    public string SubscriptionId { get; set; } = null!;

    [Required]
    public IEnumerable<string> KeyVaults { get; set; } = null!;
}
