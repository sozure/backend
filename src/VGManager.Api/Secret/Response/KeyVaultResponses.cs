using System.ComponentModel.DataAnnotations;
using VGManager.Api.Secrets.Response;
using VGManager.AzureAdapter.Entities;

namespace VGManager.Api.Secret.Response;

public class KeyVaultResponses
{
    [Required]
    public AdapterStatus Status { get; set; }

    [Required]
    public IEnumerable<string> KeyVaults { get; set; } = null!;
}
