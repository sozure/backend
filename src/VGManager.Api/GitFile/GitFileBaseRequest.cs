using System.ComponentModel.DataAnnotations;
using VGManager.Api.Common;

namespace VGManager.Api.GitFile;

public abstract class GitFileBaseRequest : GitBasicRequest
{
    [Required]
    public string Branch { get; set; } = null!;
}
