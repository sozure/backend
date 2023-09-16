using VGManager.AzureAdapter.Entities;

namespace VGManager.Api.Secrets.Response;

public class SecretResponses
{
    public Status Status { get; set; }
    public IEnumerable<SecretResponse> Secrets { get; set; } = null!;
}
