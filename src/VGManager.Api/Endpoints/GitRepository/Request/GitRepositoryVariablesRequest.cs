using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.Endpoints.GitRepository.Request;

public class GitRepositoryVariablesRequest : GitRepositoryBaseRequest
{
    [Required]
    public string RepositoryId { get; set; } = null!;

    [Required]
    public string FilePath { get; set; } = null!;

    [Required]
    public string Delimiter { get; set; } = null!;

    [Required]
    public string Branch { get; set; } = null!;

    public IEnumerable<string>? Exceptions { get; set; } = null!;
}
