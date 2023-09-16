using VGManager.AzureAdapter.Entities;

namespace VGManager.Api.Secrets.Response;

public class DeletedSecretResponses
{
    public Status Status { get; set; }
    public IEnumerable<DeletedSecretResponse> DeletedSecrets { get; set; } = null!;
}
