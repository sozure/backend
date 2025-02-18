
using System.Diagnostics.CodeAnalysis;
using CorrelationId;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using VGManager.Api.Handlers.GitFile;
using VGManager.Api.Handlers.GitPR;
using VGManager.Api.Handlers.GitRepository;
using VGManager.Api.Handlers.GitVersion;
using VGManager.Api.Handlers.Pipelines.BuildPipeline;
using VGManager.Api.Handlers.Pipelines.ReleasePipeline;
using VGManager.Api.Handlers.Profile;
using VGManager.Api.HealthChecks;
using VGManager.Communication.Kafka.Extensions;

namespace VGManager.Api;

[ExcludeFromCodeCoverage]
static partial class Program
{
    internal static WebApplication Configure(WebApplication app, string specificOrigins)
    {
        var configuration = app.Configuration;

        var healthCheckSettings = configuration
                .GetSection(Constants.SettingKeys.HealthChecksSettings)
                .Get<HealthChecksSettings>()
                ?? throw new InvalidOperationException("HealthChecksSettings is missing from configuration.");

        app.UseHealthChecks("/health/startup", healthCheckSettings.Port, new HealthCheckOptions
        {
            Predicate = healthCheck => healthCheck.Tags.Contains("startup")
        });

        app.UseHealthChecks("/health/liveness", healthCheckSettings.Port, new HealthCheckOptions
        {
            Predicate = healthCheck => healthCheck.Tags.Contains("liveness")
        });

        app.UseHealthChecks("/health/readiness", healthCheckSettings.Port, new HealthCheckOptions
        {
            Predicate = healthCheck => healthCheck.Tags.Contains("readiness")
        });

        app.UseCorrelationIdValidation();
        app.UseCorrelationId();

        RegisterStartupReadiness(app);

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
        app.UseCors(specificOrigins);
        app.UseAuthorization();

        app.MapGroup("api/azureservice")
            .RequireCors(specificOrigins)
            .MapGitRepositoryHandler()
            .MapGitPullRequestHandler()
            .MapGitVersionHandler()
            .MapGitFileHandler()
            .MapReleasePipelineHandler()
            .MapBuildPipelineHandler()
            .MapProfileHandler();

        return app;
    }

    private static void RegisterStartupReadiness(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        scope.ServiceProvider.GetRequiredService<StartupHealthCheck>().RegisterStartupReadiness();
    }
}
