namespace VGManager.Repository.Entities;
public class SecretsEntity
{
    public Status Status { get; set; }
    public IEnumerable<SecretEntity?> Secrets { get; set; } = null!;
}
