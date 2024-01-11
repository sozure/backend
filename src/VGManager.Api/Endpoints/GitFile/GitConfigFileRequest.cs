namespace VGManager.Api.Endpoints.GitFile;

public class GitConfigFileRequest : GitFileBaseRequest
{
    public string? Extension { get; set; } = null!;
}
