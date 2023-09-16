using VGManager.AzureAdapter.Entities;

namespace VGManager.Services.Models.Secrets.Results;
public class DeletedSecretResults
{
    public Status Status { get; set; }
    public IEnumerable<DeletedSecretResult> DeletedSecrets { get; set; } = null!;
}
