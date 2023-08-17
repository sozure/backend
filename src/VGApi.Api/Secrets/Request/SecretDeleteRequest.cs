namespace VGApi.Api.Secrets.Request;

public class SecretDeleteRequest
{
    public string KeyVaultName { get; set; } = null!;
    public string SecretFilter { get; set; } = null!;
}
