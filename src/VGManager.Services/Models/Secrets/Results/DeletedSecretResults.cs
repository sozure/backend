using VGManager.Models;

namespace VGManager.Services.Models.Secrets.Results;
public class DeletedSecretResults
{
    public AdapterStatus Status { get; set; }
    public IEnumerable<DeletedSecretResult> DeletedSecrets { get; set; } = null!;
}
