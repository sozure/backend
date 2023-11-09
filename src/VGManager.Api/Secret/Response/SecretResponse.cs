using System.ComponentModel.DataAnnotations;
using VGManager.Api.Secret.Response;

namespace VGManager.Api.Secrets.Response;

public class SecretResponse : SecretBaseResponse
{
    [Required]
    public string CreatedBy { get; set; } = null!;
}
