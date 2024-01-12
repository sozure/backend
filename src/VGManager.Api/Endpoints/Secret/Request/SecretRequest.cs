using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.Endpoints.Secret.Request;

public class SecretRequest : SecretBaseRequest
{
    [Required]
    public string KeyVaultName { get; set; } = null!;

    [Required]
    public string SecretFilter { get; set; } = null!;
}