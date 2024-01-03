using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.GitFile;

public class GitFileRequest
{
    [Required]
    public string Organization { get; set; } = null!;

    [Required]
    public string PAT { get; set; } = null!;

    [Required]
    public string RepositoryId { get; set; } = null!;

    [Required]
    public string FileName { get; set; } = null!;

    [Required]
    public string Branch { get; set; } = null!;
}
