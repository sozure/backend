using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VGManager.Adapter.Models.Models;
using VGManager.Adapter.Models.StatusEnums;
using VGManager.Api.Common;
using VGManager.Api.Handlers.GitVersion.Extensions;
using VGManager.Services.Interfaces;
using VGManager.Services.Models;

namespace VGManager.Api.Handlers.GitVersion;

public static class GitVersionHandler
{
    [ExcludeFromCodeCoverage]
    public static RouteGroupBuilder MapGitVersionHandler(this RouteGroupBuilder builder)
    {
        var group = builder.MapGroup("/gitversion");

        group.MapPost("/branches", GetBranchesAsync)
            .WithName("GetBranches");

        group.MapPost("/tags", GetTagsAsync)
            .WithName("GetTags");

        group.MapPost("/latesttags", GetLatestTagsAsync)
            .WithName("GetLatestTags");

        group.MapPost("/tagcreation", CreateTagAsync)
            .WithName("CreateTag");

        return builder;
    }

    public static async Task<Ok<AdapterResponseModel<IEnumerable<string>>>> GetBranchesAsync(
        [FromBody] GitBasicRequest request,
        [FromServices] IGitVersionService gitVersionService,
        CancellationToken cancellationToken
    )
    {
        try
        {
            (var status, var branches) = await gitVersionService.GetBranchesAsync(
                request.Organization,
                request.PAT,
                request.RepositoryId,
                cancellationToken
            );

            return TypedResults.Ok(new AdapterResponseModel<IEnumerable<string>>
            {
                Status = status,
                Data = branches
            });
        }
        catch (Exception)
        {
            return TypedResults.Ok(new AdapterResponseModel<IEnumerable<string>>
            {
                Status = AdapterStatus.Unknown,
                Data = []
            });
        }
    }

    public static async Task<Ok<AdapterResponseModel<IEnumerable<string>>>> GetTagsAsync(
        [FromBody] GitBasicRequest request,
        [FromServices] IGitVersionService gitVersionService,
        CancellationToken cancellationToken
    )
    {
        try
        {
            (var status, var tags) = await gitVersionService.GetTagsAsync(
                request.Organization,
                request.PAT,
                new Guid(request.RepositoryId),
                cancellationToken
                );
            return TypedResults.Ok(new AdapterResponseModel<IEnumerable<string>>
            {
                Status = status,
                Data = tags
            });
        }
        catch (Exception)
        {
            return TypedResults.Ok(new AdapterResponseModel<IEnumerable<string>>
            {
                Status = AdapterStatus.Unknown,
                Data = []
            });
        }
    }

    public static async Task<Ok<AdapterResponseModel<Dictionary<string, string>>>> GetLatestTagsAsync(
        [FromBody] GitLatestTagsRequest request,
        [FromServices] IGitVersionService gitVersionService,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var model = request.ToEntity();
            var result = await gitVersionService.GetLatestTagsAsync(model, cancellationToken);
            return TypedResults.Ok(result);
        }
        catch (Exception)
        {
            return TypedResults.Ok(new AdapterResponseModel<Dictionary<string, string>>
            {
                Status = AdapterStatus.Unknown,
                Data = []
            });
        }
    }

    public static async Task<Ok<AdapterResponseModel<string>>> CreateTagAsync(
        [FromBody] CreateTagEntity request,
        [FromServices] IGitVersionService gitVersionService,
        CancellationToken cancellationToken
    )
    {
        try
        {
            (var status, var tag) = await gitVersionService.CreateTagAsync(
                request,
                cancellationToken
                );

            return TypedResults.Ok(new AdapterResponseModel<string>
            {
                Status = status,
                Data = tag
            });
        }
        catch (Exception)
        {
            return TypedResults.Ok(new AdapterResponseModel<string>
            {
                Status = AdapterStatus.Unknown,
                Data = string.Empty
            });
        }
    }
}
