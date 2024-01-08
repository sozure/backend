using Azure.Security.KeyVault.Secrets;
using VGManager.Models;
namespace VGManager.AzureAdapter.Entities;

public class DeletedSecretsEntity
{
    public AdapterStatus Status { get; set; }
    public IEnumerable<DeletedSecret> DeletedSecrets { get; set; } = null!;
}
