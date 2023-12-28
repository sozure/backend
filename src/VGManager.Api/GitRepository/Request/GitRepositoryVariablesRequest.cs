using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.GitRepository.Request;

public class GitRepositoryVariablesRequest: GitRepositoryBaseRequest
{
    [Required]
    public string GitRepositoryId { get; set; } = null!;

    [Required]
    public string FilePath { get; set; } = null!;

    [Required]
    public string Delimiter { get; set; } = null!;
}
