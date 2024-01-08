using Azure.Security.KeyVault.Secrets;
using VGManager.Models;

namespace VGManager.AzureAdapter.Entities;
public class SecretEntity
{
    public AdapterStatus Status { get; set; }
    public KeyVaultSecret? Secret { get; set; } = null!;
}
