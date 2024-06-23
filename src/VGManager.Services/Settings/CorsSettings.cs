namespace VGManager.Services.Settings;

public record CorsSettings
{
    public string AllowedOrigin { get; set; } = null!;
}
