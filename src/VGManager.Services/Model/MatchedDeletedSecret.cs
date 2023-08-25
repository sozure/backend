namespace VGManager.Services.Model;

public class MatchedDeletedSecret
{
    public string SecretName { get; set; }
    public DateTimeOffset? DeletedOn { get; set; }

    public MatchedDeletedSecret(string secretName, DateTimeOffset? deletedOn)
    {
        SecretName = secretName;
        DeletedOn = deletedOn;
    }
}
