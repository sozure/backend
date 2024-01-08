using System.ComponentModel.DataAnnotations;

namespace VGManager.Models;

public class AdapterResponseModel<T>
{
    [Required]
    public AdapterStatus Status { get; set; }

    [Required]
    public T Data { get; set; } = default!;
}
