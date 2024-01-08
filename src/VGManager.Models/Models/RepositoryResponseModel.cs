using System.ComponentModel.DataAnnotations;
using VGManager.Models.StatusEnums;

namespace VGManager.Models.Models;

public class RepositoryResponseModel<T>
{
    [Required]
    public RepositoryStatus Status { get; set; }

    [Required]
    public IEnumerable<T> Data { get; set; } = null!;
}
