namespace VGManager.Services.Models;

public class MatchedDeletedSecret
{
    public string SecretName { get; set; } = null!;
    public DateTimeOffset? DeletedOn { get; set; }
}
