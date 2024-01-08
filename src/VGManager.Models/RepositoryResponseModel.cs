using System.ComponentModel.DataAnnotations;

namespace VGManager.Models;

public class RepositoryResponseModel<T>
{
    [Required]
    public RepositoryStatus Status { get; set; }

    [Required]
    public IEnumerable<T> Data { get; set; } = null!;
}
