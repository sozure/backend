using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.Endpoints.Secret.Response;

public class SecretResponse : SecretBaseResponse
{
    [Required]
    public string CreatedBy { get; set; } = null!;
}
