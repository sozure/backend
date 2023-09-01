using VGManager.Repository.Entities;
using VGManager.Services.Models.Secrets;

namespace VGManager.Api.Secrets.Response;

public class DeletedSecretsGetResponse
{
    public Status Status { get; set; }
    public IEnumerable<DeletedSecretGetResponse> DeletedSecrets { get; set; } = null!;
}
