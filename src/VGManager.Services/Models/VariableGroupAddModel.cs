
using System.ComponentModel.DataAnnotations;

namespace VGManager.Services.Models;

public class VariableGroupAddModel : VariableGroupModel
{
    [Required]
    public string Key { get; set; } = null!;

    [Required]
    public string Value { get; set; } = null!;
}
