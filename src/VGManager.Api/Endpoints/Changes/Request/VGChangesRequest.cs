using System.ComponentModel.DataAnnotations;
using VGManager.Services.Models.Changes;

namespace VGManager.Api.Endpoints.Changes.Request;

public class VGChangesRequest : BaseRequest
{
    [Required]
    public string Organization { get; set; } = null!;
    [Required]
    public string Project { get; set; } = null!;
    [Required]
    public IEnumerable<ChangeType> ChangeTypes { get; set; } = Array.Empty<ChangeType>();
}
