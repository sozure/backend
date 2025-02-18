using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VGManager.Adapter.Models.Models;
using VGManager.Adapter.Models.StatusEnums;
using VGManager.Api.Common;
using VGManager.Services.Interfaces;

namespace VGManager.Api.Handlers.Pipelines.BuildPipeline;

public static class BuildPipelineHandler
{
    [ExcludeFromCodeCoverage]
    public static RouteGroupBuilder MapBuildPipelineHandler(this RouteGroupBuilder builder)
    {
        var group = builder.MapGroup("/buildpipeline");

        group.MapPost("/getrepositoryid", GetRepositoryIdAsync)
            .WithName("GetRepositoryId");

        group.MapPost("/getall", GetAllAsync)
           .WithName("GetAll");

        group.MapPost("/run", RunBuildPipelineAsync)
           .WithName("RunBuildPipeline");

        group.MapPost("/runall", RunBuildPipelinesAsync)
           .WithName("RunBuildPipelines");

        return builder;
    }

    public static async Task<Ok<AdapterResponseModel<Guid>>> GetRepositoryIdAsync(
        [FromBody] BuildPipelineRequest request,
        [FromServices] IBuildPipelineService buildPipelineService,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var id = await buildPipelineService.GetRepositoryIdByBuildDefinitionAsync(
                request.Organization,
                request.PAT,
                request.Project,
                request.DefinitionId,
                cancellationToken
                );
            return TypedResults.Ok(new AdapterResponseModel<Guid>()
            {
                Status = AdapterStatus.Success,
                Data = id
            });
        }
        catch (Exception)
        {
            return TypedResults.Ok(new AdapterResponseModel<Guid>()
            {
                Status = AdapterStatus.Unknown,
                Data = Guid.Empty
            });
        }
    }

    public static async Task<Ok<AdapterResponseModel<IEnumerable<Dictionary<string, string>>>>> GetAllAsync(
        [FromBody] ExtendedBasicRequest request,
        [FromServices] IBuildPipelineService buildPipelineService,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var pipelines = await buildPipelineService.GetBuildPipelinesAsync(
                request.Organization,
                request.PAT,
                request.Project,
                cancellationToken
                );
            return TypedResults.Ok(new AdapterResponseModel<IEnumerable<Dictionary<string, string>>>()
            {
                Status = AdapterStatus.Success,
                Data = pipelines
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

    public static async Task<Ok<AdapterStatus>> RunBuildPipelineAsync(
        [FromBody] RunBuildRequest request,
        [FromServices] IBuildPipelineService buildPipelineService,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var status = await buildPipelineService.RunBuildPipelineAsync(
                request.Organization,
                request.PAT,
                request.Project,
                request.DefinitionId,
                request.SourceBranch,
                cancellationToken
                );
            return TypedResults.Ok(status);
        }
        catch (Exception)
        {
            return TypedResults.Ok(AdapterStatus.Unknown);
        }
    }

    public static async Task<Ok<AdapterStatus>> RunBuildPipelinesAsync(
        [FromBody] RunBuildPipelinesRequest request,
        [FromServices] IBuildPipelineService pipelineService,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var status = await pipelineService.RunBuildPipelinesAsync(
                request.Organization,
                request.PAT,
                request.Project,
                request.Pipelines,
                cancellationToken
                );

            return TypedResults.Ok(status);
        }
        catch (Exception)
        {
            return TypedResults.Ok(AdapterStatus.Unknown);
        }
    }
}
