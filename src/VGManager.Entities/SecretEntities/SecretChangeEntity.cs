namespace VGManager.Entities.SecretEntities;

public class SecretChangeEntity: SecretEntity
{
    public string KeyVaultName { get; set; } = null!;
    public string SecretNameRegex { get; set; } = null!;
    public SecretChangeType ChangeType { get; set; }
}
