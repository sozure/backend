namespace VGManager.AzureAdapter.Entities;
public class SecretsEntity
{
    public AdapterStatus Status { get; set; }
    public IEnumerable<SecretEntity> Secrets { get; set; } = null!;
}
