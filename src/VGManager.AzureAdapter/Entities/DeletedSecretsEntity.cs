using Azure.Security.KeyVault.Secrets;

namespace VGManager.AzureAdapter.Entities;

public class DeletedSecretsEntity
{
    public AdapterStatus Status { get; set; }
    public IEnumerable<DeletedSecret> DeletedSecrets { get; set; } = null!;
}
