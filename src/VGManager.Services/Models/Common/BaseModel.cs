namespace VGManager.Services.Models.Common;

public record BaseModel
{
    public string Organization { get; set; } = null!;
    public string PAT { get; set; } = null!;
}
