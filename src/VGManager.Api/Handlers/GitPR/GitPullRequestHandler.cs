using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VGManager.Adapter.Models.Models;
using VGManager.Adapter.Models.Requests;
using VGManager.Adapter.Models.Response;
using VGManager.Services.Interfaces;

namespace VGManager.Api.Handlers.GitPR;

public static class GitPullRequestHandler
{
    [ExcludeFromCodeCoverage]
    public static RouteGroupBuilder MapGitPullRequestHandler(this RouteGroupBuilder builder)
    {
        var group = builder.MapGroup("/gitpullrequest");

        group.MapPost("/getprs", GetAsync)
            .WithName("GetPullRequests");

        group.MapPost("/createpr", CreatePullRequestAsync)
            .WithName("CreatePullRequest");

        group.MapPost("/createprs", CreatePullRequestsAsync)
            .WithName("CreatePullRequests");

        return builder;
    }

    public static async Task<Ok<AdapterResponseModel<List<GitPRResponse>>>> GetAsync(
        [FromBody] GitPRRequest request,
        [FromServices] IPullRequestService pullRequestService,
        CancellationToken cancellationToken
    )
    {
        var gitPullRequests = await pullRequestService.GetPullRequestsAsync(request, cancellationToken);
        return TypedResults.Ok(gitPullRequests);
    }

    public static async Task<Ok<AdapterResponseModel<bool>>> CreatePullRequestAsync(
        [FromBody] CreatePRRequest request,
        [FromServices] IPullRequestService pullRequestService,
        CancellationToken cancellationToken
    )
    {
        var result = await pullRequestService.CreatePullRequestAsync(request, cancellationToken);
        return TypedResults.Ok(result);
    }

    public static async Task<Ok<AdapterResponseModel<bool>>> CreatePullRequestsAsync(
        [FromBody] CreatePRsRequest request,
        [FromServices] IPullRequestService pullRequestService,
        CancellationToken cancellationToken
    )
    {
        var result = await pullRequestService.CreatePullRequestsAsync(request, cancellationToken);
        return TypedResults.Ok(result);
    }
}
