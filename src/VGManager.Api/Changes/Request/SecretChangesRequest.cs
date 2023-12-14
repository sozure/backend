using System.ComponentModel.DataAnnotations;
using VGManager.Services.Models.Changes;

namespace VGManager.Api.Changes.Request;

public class SecretChangesRequest
{
    [Required]
    public string KeyVaultName { get; set; } = null!;
    [Required]
    public DateTime From { get; set; }
    [Required]
    public DateTime To { get; set; }
    [Required]
    public int Limit { get; set; }
    [Required]
    public IEnumerable<ChangeType> ChangeTypes { get; set; } = Array.Empty<ChangeType>();
    public string? User { get; set; } = null!;
}
