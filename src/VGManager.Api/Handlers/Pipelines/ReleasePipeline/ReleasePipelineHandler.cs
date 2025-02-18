using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VGManager.Adapter.Models.Models;
using VGManager.Adapter.Models.StatusEnums;
using VGManager.Api.Handlers.Pipelines.Release.Request;
using VGManager.Services.Interfaces;

namespace VGManager.Api.Handlers.Pipelines.ReleasePipeline;

public static class ReleasePipelineHandler
{
    [ExcludeFromCodeCoverage]
    public static RouteGroupBuilder MapReleasePipelineHandler(this RouteGroupBuilder builder)
    {
        var group = builder.MapGroup("/releasepipeline");

        group.MapPost("/environments", GetEnvironmentsAsync)
            .WithName("GetEnvironments");

        group.MapPost("/variableGroups", GetVariableGroupsAsync)
            .WithName("GetVariableGroups");

        group.MapPost("/projects", GetProjectsWithReleasePipelineAsync)
            .WithName("GetProjects");

        return builder;
    }

    public static async Task<Ok<AdapterResponseModel<IEnumerable<string>>>> GetEnvironmentsAsync(
        [FromBody] ReleasePipelineRequest request,
        [FromServices] IReleasePipelineService releasePipelineService,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var (status, environments) = await releasePipelineService.GetEnvironmentsAsync(
                request.Organization,
                request.PAT,
                request.Project,
                request.RepositoryName,
                request.ConfigFile,
                cancellationToken
                );

            return TypedResults.Ok(new AdapterResponseModel<IEnumerable<string>>()
            {
                Status = status,
                Data = environments
            });
        }
        catch (Exception)
        {
            return TypedResults.Ok(new AdapterResponseModel<IEnumerable<string>>()
            {
                Status = AdapterStatus.Unknown,
                Data = []
            });
        }
    }

    public static async Task<Ok<AdapterResponseModel<IEnumerable<Dictionary<string, string>>>>> GetVariableGroupsAsync(
        [FromBody] ReleasePipelineRequest request,
        [FromServices] IReleasePipelineService releasePipelineService,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var (status, variableGroups) = await releasePipelineService.GetVariableGroupsAsync(
                request.Organization,
                request.PAT,
                request.Project,
                request.RepositoryName,
                request.ConfigFile,
                cancellationToken
                );

            var result = new List<Dictionary<string, string>>();

            foreach (var (name, type) in variableGroups)
            {
                var dictionary = new Dictionary<string, string>
                {
                    { "Name", name },
                    { "Type", type }
                };
                result.Add(dictionary);
            }

            return TypedResults.Ok(new AdapterResponseModel<IEnumerable<Dictionary<string, string>>>()
            {
                Status = status,
                Data = result
            });
        }
        catch (Exception)
        {
            return TypedResults.Ok(new AdapterResponseModel<IEnumerable<Dictionary<string, string>>>()
            {
                Status = AdapterStatus.Unknown,
                Data = []
            });
        }
    }

    public static async Task<Ok<AdapterResponseModel<IEnumerable<string>>>> GetProjectsWithReleasePipelineAsync(
        [FromBody] ProjectsWithReleasePipelineRequest request,
        [FromServices] IReleasePipelineService releasePipelineService,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var (status, projects) = await releasePipelineService.GetProjectsWithReleasePipelineAsync(
                request.Organization,
                request.PAT,
                request.Projects,
                request.RepositoryName,
                request.ConfigFile,
                cancellationToken
                );

            return TypedResults.Ok(new AdapterResponseModel<IEnumerable<string>>()
            {
                Status = status,
                Data = projects
            });
        }
        catch (Exception)
        {
            return TypedResults.Ok(new AdapterResponseModel<IEnumerable<string>>()
            {
                Status = AdapterStatus.Unknown,
                Data = []
            });
        }
    }
}
