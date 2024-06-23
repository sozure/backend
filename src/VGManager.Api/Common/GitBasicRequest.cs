using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.Common;

public record GitBasicRequest : BasicRequest
{
    [Required]
    public string RepositoryId { get; set; } = null!;
}
