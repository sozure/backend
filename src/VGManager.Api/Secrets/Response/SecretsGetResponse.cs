using VGManager.Repository.Entities;

namespace VGManager.Api.Secrets.Response;

public class SecretsGetResponse
{
    public Status Status { get; set; }
    public IEnumerable<SecretGetResponse> Secrets { get; set; } = null!;
}
