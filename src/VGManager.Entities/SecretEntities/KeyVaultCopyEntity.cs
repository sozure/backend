namespace VGManager.Entities.SecretEntities;

public class KeyVaultCopyEntity: SecretEntity
{
    public string OriginalKeyVault { get; set; } = null!;
    public string DestinationKeyVault { get; set; } = null!;
}
