namespace VGManager.Api.Secrets.Response;

public class DeletedSecretGetResponse
{
    public string SecretName { get; set; } = null!;
    public string SecretValue { get; set; } = null!;
    public DateTimeOffset? DeletedOn { get; set; }
}
