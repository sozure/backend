namespace VGApi.Api.Secrets.Response;

public class SecretGetResponse
{
    public string SecretName { get; set; }
    public string SecretValue { get; set; }
    public string CreatedBy { get; set; }
}
