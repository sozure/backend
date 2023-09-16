using VGManager.Api.Secret.Response;

namespace VGManager.Api.Secrets.Response;

public class SecretResponse : SecretBaseResponse
{
    public string CreatedBy { get; set; } = null!;
}
