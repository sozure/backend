namespace VGManager.Services.Models.Secrets;
public class SecretModel : SecretBaseModel
{
    public string KeyVaultName { get; set; } = null!;
    public string SecretFilter { get; set; } = null!;
}
