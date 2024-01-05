using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.GitFile;

public class GitConfigFileRequest: GitFileBaseRequest
{
    [Required]
    public string Extension { get; set; } = null!;
}
