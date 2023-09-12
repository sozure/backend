namespace VGManager.Services.Models.Secrets;
public class SecretCopyModel : SecretBaseModel
{
    public string FromKeyVault { get; set; } = null!;

    public string ToKeyVault { get; set; } = null!;

    public bool overrideSecret { get; set; }
}
