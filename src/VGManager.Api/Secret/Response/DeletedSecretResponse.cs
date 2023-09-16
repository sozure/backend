using VGManager.Api.Secret.Response;

namespace VGManager.Api.Secrets.Response;

public class DeletedSecretResponse : SecretBaseResponse
{
    public DateTimeOffset? DeletedOn { get; set; }
}
