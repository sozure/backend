using System.ComponentModel.DataAnnotations;
using VGManager.Services.Models.Changes;

namespace VGManager.Api.Changes.Request;

public class SecretChangesRequest : BaseRequest
{
    [Required]
    public string KeyVaultName { get; set; } = null!;
    [Required]
    public IEnumerable<ChangeType> ChangeTypes { get; set; } = Array.Empty<ChangeType>();
}
