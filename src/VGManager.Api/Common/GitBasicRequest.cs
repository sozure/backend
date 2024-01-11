using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.Common;

public class GitBasicRequest : BasicRequest
{
    [Required]
    public string RepositoryId { get; set; } = null!;
}
