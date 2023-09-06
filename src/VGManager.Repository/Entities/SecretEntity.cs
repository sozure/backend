using Azure.Security.KeyVault.Secrets;

namespace VGManager.Repository.Entities;
public class SecretEntity
{
    public Status Status { get; set; }
    public KeyVaultSecret? Secret { get; set; } = null!;
}
