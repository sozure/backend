using VGManager.Repository.Entities;

namespace VGManager.Services.Models.Secrets;
public class DeletedSecretResultsModel
{
    public Status Status { get; set; }
    public IEnumerable<DeletedSecretResultModel> DeletedSecrets { get; set; } = null!;
}
