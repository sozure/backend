namespace VGManager.Services.Models;

public class CreateTagEntity
{
    public string Organization { get; set; } = null!;
    public string PAT { get; set; } = null!;
    public string Project { get; set; } = null!;
    public Guid RepositoryId { get; set; }
    public string TagName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string? Description { get; set; } = null!;
}
