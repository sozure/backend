using Azure.Security.KeyVault.Secrets;

namespace VGManager.AzureAdapter.Entities;
public class SecretEntity
{
    public AdapterStatus Status { get; set; }
    public KeyVaultSecret? Secret { get; set; } = null!;
}
