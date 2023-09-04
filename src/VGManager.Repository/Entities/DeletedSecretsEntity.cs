using Azure.Security.KeyVault.Secrets;

namespace VGManager.Repository.Entities;

public class DeletedSecretsEntity
{
    public Status Status { get; set; }
    public IEnumerable<DeletedSecret> DeletedSecrets { get; set; } = null!;
}
