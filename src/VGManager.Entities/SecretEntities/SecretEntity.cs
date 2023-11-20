namespace VGManager.Entities.SecretEntities;

public abstract class SecretEntity
{
    public string Id { get; set; } = null!;
    public string User { get; set; } = null!;
    public DateTime Date { get; set; }
}
