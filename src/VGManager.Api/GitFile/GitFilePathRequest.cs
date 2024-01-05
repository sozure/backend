using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.GitFile;

public class GitFilePathRequest: GitFileBaseRequest
{
    [Required]
    public string FileName { get; set; } = null!;
}
