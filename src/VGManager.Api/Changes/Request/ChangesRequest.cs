using System.ComponentModel.DataAnnotations;
using VGManager.Services.Models.Changes;

namespace VGManager.Api.Changes.Request;

public class ChangesRequest
{
    [Required]
    public ChangeType[] ChangeTypes { get; set; } = Array.Empty<ChangeType>();
}
