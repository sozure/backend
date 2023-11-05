namespace VGManager.Repositories;

public class DatabaseConfiguration
{
    public string ProviderKey { get; set; } = null!;
    public string PostgreMigrationsAssemblyKey { get; set; } = null!;
    public string PostgreConnectionStringKey { get; set; } = null!;
}
