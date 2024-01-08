using System.ComponentModel.DataAnnotations;
using VGManager.Api.Common;

namespace VGManager.Api.GitRepository.Request;

public class GitRepositoryBaseRequest : BasicRequest
{
    [Required]
    public string Project { get; set; } = null!;
}
