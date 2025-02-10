using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VGManager.Adapter.Models.Models;
using VGManager.Api.Handlers.GitRepository.Request;
using VGManager.Services.Interfaces;
using VGManager.Services.Models.GitRepositories;

namespace VGManager.Api.Handlers.GitRepository;

public static class GitRepositoryHandler
{
    [ExcludeFromCodeCoverage]
    public static RouteGroupBuilder MapGitRepositoryHandler(this RouteGroupBuilder builder)
    {
        var group = builder.MapGroup("/gitrepository");

        group.MapPost("/", GetAsync)
            .WithName("GetRepositories");

        group.MapPost("/variables", GetVariablesAsync)
            .WithName("GetVariables");

        return builder;
    }

    public static async Task<Ok<AdapterResponseModel<IEnumerable<GitRepositoryResult>>>> GetAsync(
        [FromBody] GitRepositoryBaseRequest request,
        [FromServices] IGitRepositoryService gitRepositoryService,
        CancellationToken cancellationToken
    )
    {
        var gitRepositories = await gitRepositoryService.GetAllAsync(
            request.Organization,
            request.Project,
            request.PAT,
            cancellationToken
            );

        var result = new AdapterResponseModel<IEnumerable<GitRepositoryResult>>()
        {
            Status = gitRepositories.Status,
            Data = gitRepositories.Data
        };

        return TypedResults.Ok(result);
    }

    public static async Task<Ok<AdapterResponseModel<IEnumerable<string>>>> GetVariablesAsync(
        [FromBody] GitRepositoryVariablesRequest request,
        [FromServices] IGitRepositoryService gitRepositoryService,
        IMapper mapper,
        CancellationToken cancellationToken
    )
    {
        var model = mapper.Map<GitRepositoryModel>(request);
        var variablesResult = await gitRepositoryService.GetVariablesFromConfigAsync(model, cancellationToken);
        var result = new AdapterResponseModel<IEnumerable<string>>()
        {
            Status = variablesResult.Status,
            Data = variablesResult.Data
        };
        return TypedResults.Ok(result);
    }
}
