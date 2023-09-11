using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.Secrets.Request;

public class SecretGetRequest
{
    [Required]
    public string TenantId { get; set; } = null!;

    [Required]
    public string ClientId { get; set; } = null!;

    [Required]
    public string ClientSecret { get; set; } = null!;

    [Required]
    public string KeyVaultName { get; set; } = null!;

    [Required]
    public string SecretFilter { get; set; } = null!;
}
