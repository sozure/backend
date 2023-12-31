using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using System.Reflection;
using VGManager.Api;
using VGManager.Api.HealthChecks;
using VGManager.AzureAdapter;
using VGManager.AzureAdapter.Interfaces;
using VGManager.Repositories.Boilerplate;
using VGManager.Repositories.DbContexts;
using VGManager.Repositories.Interfaces.SecretRepositories;
using VGManager.Repositories.Interfaces.VGRepositories;
using VGManager.Repositories.SecretRepositories;
using VGManager.Repositories.VGRepositories;
using VGManager.Services;
using VGManager.Services.Interfaces;
using VGManager.Services.Settings;
using ServiceProfiles = VGManager.Services.MapperProfiles;

static partial class Program
{
    public static WebApplicationBuilder ConfigureServices(WebApplicationBuilder self, string specificOrigins)
    {
        var configuration = self.Configuration;
        var services = self.Services;

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
            .AddCheck<StartupHealthCheck>(nameof(StartupHealthCheck), tags: new[] { "startup" });

        services.AddAutoMapper(
            typeof(Program),
            typeof(ServiceProfiles.ProjectProfile)
        );

        services.AddOptions<OrganizationSettings>()
            .Bind(configuration.GetSection(Constants.SettingKeys.OrganizationSettings))
            .ValidateDataAnnotations();

        RegisterServices(services, configuration);

        return self;
    }

    private static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        var databaseProviderKey = "DatabaseProvider";
        services.AddSingleton<StartupHealthCheck>();

        var dbConfig = new DatabaseConfiguration
        {
            ProviderKey = databaseProviderKey,
            PostgreConnectionStringKey = Constants.ConnectionStringKeys.PostgreSql,
            PostgreMigrationsAssemblyKey = Constants.MigrationAssemblyNames.PostgreSql,
        };

        services.AddDbContext<OperationsDbContext>(delegate (DbContextOptionsBuilder options)
        {
            options.UseNpgsql(
                configuration.GetConnectionString(dbConfig.PostgreConnectionStringKey),
                delegate (NpgsqlDbContextOptionsBuilder options)
                {
                    options.MigrationsAssembly(dbConfig.PostgreMigrationsAssemblyKey);
                }
            );
        }, ServiceLifetime.Scoped);

        services.AddScoped<IVGAddColdRepository, VGAddColdRepository>();
        services.AddScoped<IVGDeleteColdRepository, VGDeleteColdRepository>();
        services.AddScoped<IVGUpdateColdRepository, VGUpdateColdRepository>();
        services.AddScoped<IKeyVaultCopyColdRepository, KeyVaultCopyColdRepository>();
        services.AddScoped<ISecretChangeColdRepository, SecretChangeColdRepository>();
        services.AddScoped<IVariableService, VariableService>();
        services.AddScoped<IVariableFilterService, VariableFilterService>();
        services.AddScoped<IVariableGroupService, VariableGroupService>();
        services.AddScoped<IKeyVaultService, KeyVaultService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IGitRepositoryService, GitRepositoryService>();
        services.AddScoped<IGitBranchService, GitBranchService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IChangeService, ChangeService>();
        services.AddScoped<IProfileAdapter, ProfileAdapter>();
        services.AddScoped<IVariableGroupAdapter, VariableGroupAdapter>();
        services.AddScoped<IProjectAdapter, ProjectAdapter>();
        services.AddScoped<IKeyVaultAdapter, KeyVaultAdapter>();
        services.AddScoped<IGitRepositoryAdapter, GitRepositoryAdapter>();
        services.AddScoped<IGitBranchAdapter, GitBranchAdapter>();
    }
}
