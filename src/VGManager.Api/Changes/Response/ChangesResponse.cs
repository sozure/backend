using System.ComponentModel.DataAnnotations;
using VGManager.Entities;
using VGManager.Services.Models.Changes;

namespace VGManager.Api.Changes.Response;

public class ChangesResponse
{
    [Required]
    public RepositoryStatus Status { get; set; }
    [Required]
    public IEnumerable<OperationModel> Operations { get; set; } = Array.Empty<OperationModel>();
}
