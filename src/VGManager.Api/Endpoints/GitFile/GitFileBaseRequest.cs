using System.ComponentModel.DataAnnotations;
using VGManager.Api.Common;

namespace VGManager.Api.Endpoints.GitFile;

public abstract record GitFileBaseRequest : GitBasicRequest
{
    [Required]
    public string Branch { get; set; } = null!;
}
