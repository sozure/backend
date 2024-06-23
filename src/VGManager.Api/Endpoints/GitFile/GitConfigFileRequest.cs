namespace VGManager.Api.Endpoints.GitFile;

public record GitConfigFileRequest : GitFileBaseRequest
{
    public string? Extension { get; set; } = null!;
}
