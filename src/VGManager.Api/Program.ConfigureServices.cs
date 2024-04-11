using CorrelationId.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;
using VGManager.Adapter.Client.Extensions;
using VGManager.Api.HealthChecks;
using VGManager.Services;
using VGManager.Services.Helper;
using VGManager.Services.Interfaces;

namespace VGManager.Api;

static partial class Program
{
    private static readonly string[] Tags = ["startup"];

    public static WebApplicationBuilder ConfigureServices(WebApplicationBuilder self, string specificOrigins)
    {
        var configuration = self.Configuration;
        var services = self.Services;

        services.AddDefaultCorrelationId(options =>
        {
            options.AddToLoggingScope = true;
        });

        services.AddCors(options =>
        {
            options.AddPolicy(name: specificOrigins,
                                policy =>
                                {
                                    policy.WithOrigins("http://localhost:3000")
                                    .AllowAnyMethod()
                                    .AllowAnyHeader();
                                });
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "VGManager.Api", Version = "v1" });
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
            c.UseOneOfForPolymorphism();
        });

        services.AddAuthorization();
        services.AddControllers();
        services.AddHealthChecks()
            .AddCheck<StartupHealthCheck>(nameof(StartupHealthCheck), tags: Tags);

        services.AddAutoMapper(
            typeof(Program)
        );

        RegisterServices(services, configuration);

        return self;
    }

    private static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<StartupHealthCheck>();
        services.SetupVGManagerAdapterClient(configuration);

        services.AddScoped<IGitAdapterCommunicatorService, GitAdapterCommunicatorService>();
        services.AddScoped<IGitRepositoryService, GitRepositoryService>();
        services.AddScoped<IGitVersionService, GitVersionService>();
        services.AddScoped<IGitFileService, GitFileService>();
        services.AddScoped<IReleasePipelineService, ReleasePipelineService>();
        services.AddScoped<IBuildPipelineService, BuildPipelineService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IAdapterCommunicator, AdapterCommunicator>();
        services.AddScoped<IGitPullRequestService, GitPullRequestService>();
    }
}
