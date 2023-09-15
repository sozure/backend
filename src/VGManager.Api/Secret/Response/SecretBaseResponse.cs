namespace VGManager.Api.Secret.Response;

public abstract class SecretBaseResponse
{
    public string KeyVault { get; set; } = null!;
    public string SecretName { get; set; } = null!;
    public string SecretValue { get; set; } = null!;
}
