namespace VGManager.Api.Handlers.GitFile;

public record GitConfigFileRequest : GitFileBaseRequest
{
    public string? Extension { get; set; } = null!;
}
