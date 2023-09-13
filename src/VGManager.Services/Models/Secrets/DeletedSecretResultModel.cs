namespace VGManager.Services.Models.Secrets;

public class DeletedSecretResultModel
{
    public string SecretName { get; set; } = null!;
    public string SecretValue { get; set; } = null!;

    public DateTimeOffset? DeletedOn { get; set; }
}
