namespace VGManager.Services.Models.MatchedModels;

public class MatchedDeletedSecret
{
    public string SecretName { get; set; } = null!;
    public DateTimeOffset? DeletedOn { get; set; }
}
