using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.Handlers.GitFile;

public record GitFilePathRequest : GitFileBaseRequest
{
    [Required]
    public string FileName { get; set; } = null!;
}
