using Azure.Security.KeyVault.Secrets;

namespace VGManager.AzureAdapter.Entities;

public class DeletedSecretsEntity
{
    public Status Status { get; set; }
    public IEnumerable<DeletedSecret> DeletedSecrets { get; set; } = null!;
}
