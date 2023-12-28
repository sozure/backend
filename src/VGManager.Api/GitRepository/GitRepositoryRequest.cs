using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.GitRepository;

public class GitRepositoryRequest
{
    [Required]
    public string Organization { get; set; } = null!;

    [Required]
    public string Project { get; set; } = null!;

    [Required]
    public string PAT { get; set; } = null!;
}
