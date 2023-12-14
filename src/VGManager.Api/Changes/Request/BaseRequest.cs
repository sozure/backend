using System.ComponentModel.DataAnnotations;
using VGManager.Services.Models.Changes;

namespace VGManager.Api.Changes.Request;

public abstract class BaseRequest
{
    [Required]
    public DateTime From { get; set; }
    [Required]
    public DateTime To { get; set; }
    [Required]
    public int Limit { get; set; }
    [Required]
    public string? User { get; set; } = null!;
}
