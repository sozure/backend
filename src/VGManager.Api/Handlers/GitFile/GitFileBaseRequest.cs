using System.ComponentModel.DataAnnotations;
using VGManager.Api.Common;

namespace VGManager.Api.Handlers.GitFile;

public abstract record GitFileBaseRequest : GitBasicRequest
{
    [Required]
    public string Branch { get; set; } = null!;
}
