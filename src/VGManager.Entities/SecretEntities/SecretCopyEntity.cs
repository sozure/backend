namespace VGManager.Entities.SecretEntities;

public class SecretCopyEntity: SecretEntity
{
    public string OriginalKeyVault { get; set; } = null!;
    public string DestinationKeyVault { get; set; } = null!;
}
