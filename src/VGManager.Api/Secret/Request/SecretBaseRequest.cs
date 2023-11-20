using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.Secret.Request;

public class SecretBaseRequest
{
    [Required]
    public string TenantId { get; set; } = null!;

    [Required]
    public string ClientId { get; set; } = null!;

    [Required]
    public string ClientSecret { get; set; } = null!;
}
