using System.ComponentModel.DataAnnotations;
using VGManager.Entities;
using VGManager.Services.Models.Changes.Responses;

namespace VGManager.Api.Changes.Response;

public class SecretChangesResponse
{
    [Required]
    public RepositoryStatus Status { get; set; }
    [Required]
    public IEnumerable<SecretOperationModel> Operations { get; set; } = Array.Empty<SecretOperationModel>();
}
