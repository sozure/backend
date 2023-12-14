using System.ComponentModel.DataAnnotations;
using VGManager.Entities;
using VGManager.Services.Models.Changes.Responses;

namespace VGManager.Api.Changes.Response;

public class VGChangesResponse
{
    [Required]
    public RepositoryStatus Status { get; set; }
    [Required]
    public IEnumerable<VGOperationModel> Operations { get; set; } = Array.Empty<VGOperationModel>();
}
