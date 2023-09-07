using VGManager.AzureAdapter.Entities;

namespace VGManager.Api.Secrets.Response;

public class DeletedSecretsGetResponse
{
    public Status Status { get; set; }
    public IEnumerable<DeletedSecretGetResponse> DeletedSecrets { get; set; } = null!;
}
