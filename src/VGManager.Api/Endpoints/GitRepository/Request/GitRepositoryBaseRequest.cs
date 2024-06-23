using System.ComponentModel.DataAnnotations;
using VGManager.Api.Common;

namespace VGManager.Api.Endpoints.GitRepository.Request;

public record GitRepositoryBaseRequest : BasicRequest
{
    [Required]
    public string Project { get; set; } = null!;
}
