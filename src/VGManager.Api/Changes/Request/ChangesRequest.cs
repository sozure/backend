using System.ComponentModel.DataAnnotations;
using VGManager.Services.Models.Changes;

namespace VGManager.Api.Changes.Request;

public class ChangesRequest
{
    [Required]
    public DateTime From { get; set; }
    [Required]
    public DateTime To { get; set; }
    [Required]
    public IEnumerable<ChangeType> ChangeTypes { get; set; } = Array.Empty<ChangeType>();
}
