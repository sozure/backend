using VGManager.Repository.Entities;
using VGManager.Services.Models.Secrets;

namespace VGManager.Api.Secrets.Response;

public class SecretsGetResponse
{
    public Status Status { get; set; }
    public IEnumerable<SecretGetResponse> Secrets { get; set; } = null!;
}
