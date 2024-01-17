using System.ComponentModel.DataAnnotations;

namespace VGManager.Api.Endpoints.GitFile;

public class GitFilePathRequest : GitFileBaseRequest
{
    [Required]
    public string FileName { get; set; } = null!;
}
