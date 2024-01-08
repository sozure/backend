using System.ComponentModel.DataAnnotations;
using VGManager.Models;

namespace VGManager.Api.Secrets.Response;

public class DeletedSecretResponses
{
    [Required]
    public AdapterStatus Status { get; set; }

    [Required]
    public IEnumerable<DeletedSecretResponse> DeletedSecrets { get; set; } = null!;
}
