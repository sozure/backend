namespace VGManager.Api.Secrets.Response;

public class SecretGetResponse
{
    public string SecretName { get; set; } = null!;
    public string SecretValue { get; set; } = null!;
    public string CreatedBy { get; set; } = null!;
}
