using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.Secret.Request;

public class SecretCopyRequest : SecretBaseRequest
{
    [Required]
    public string FromKeyVault { get; set; } = null!;

    [Required]
    public string ToKeyVault { get; set; } = null!;

    [Required]
    public bool OverrideSecret { get; set; }
}
