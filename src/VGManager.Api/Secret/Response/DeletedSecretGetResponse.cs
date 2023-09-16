using VGManager.Api.Secret.Response;

namespace VGManager.Api.Secrets.Response;

public class DeletedSecretGetResponse : SecretBaseResponse
{
    public DateTimeOffset? DeletedOn { get; set; }
}
