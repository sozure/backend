using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VGManager.Adapter.Models.Models;
using VGManager.Services.Interfaces;

namespace VGManager.Api.Handlers.GitFile;

public static class GitFileHandler
{
    [ExcludeFromCodeCoverage]
    public static RouteGroupBuilder MapGitFileHandler(this RouteGroupBuilder builder)
    {
        var group = builder.MapGroup("/gitfile");

        group.MapPost("/path", GetFilePathAsync)
            .WithName("GetFilePath");

        group.MapPost("/config", GetConfigFilesAsync)
            .WithName("GetConfigFiles");

        return builder;
    }

    public static async Task<Ok<AdapterResponseModel<IEnumerable<string>>>> GetFilePathAsync(
        [FromBody] GitFilePathRequest request,
        [FromServices] IGitFileService gitFileService,
        CancellationToken cancellationToken
    )
    {
        (var status, var filePaths) = await gitFileService.GetFilePathAsync(
            request.Organization,
            request.PAT,
            request.RepositoryId,
            request.FileName,
            request.Branch,
            cancellationToken
        );

        var result = new AdapterResponseModel<IEnumerable<string>>
        {
            Status = status,
            Data = filePaths
        };
        return TypedResults.Ok(result);
    }

    public static async Task<Ok<AdapterResponseModel<IEnumerable<string>>>> GetConfigFilesAsync(
        [FromBody] GitConfigFileRequest request,
        [FromServices] IGitFileService gitFileService,
        CancellationToken cancellationToken
    )
    {
        (var status, var configFiles) = await gitFileService.GetConfigFilesAsync(
            request.Organization,
            request.PAT,
            request.RepositoryId,
            request.Extension,
            request.Branch,
            cancellationToken
        );

        var result = new AdapterResponseModel<IEnumerable<string>>
        {
            Status = status,
            Data = configFiles
        };

        return TypedResults.Ok(result);
    }
}
