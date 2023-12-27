using System.ComponentModel.DataAnnotations;
using VGManager.Entities;
using VGManager.Services.Models.Changes.Responses;

namespace VGManager.Api.Changes.Response;

public class KVChangesResponse
{
    [Required]
    public RepositoryStatus Status { get; set; }
    [Required]
    public IEnumerable<KVOperationModel> Operations { get; set; } = Array.Empty<KVOperationModel>();
}
