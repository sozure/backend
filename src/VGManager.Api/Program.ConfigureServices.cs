using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using VGManager.Api.HealthChecks;
using VGManager.Api.MapperProfiles;
using VGManager.Repositories;
using VGManager.Repository.DbContexts;
using ServiceProfiles = VGManager.Services.MapperProfiles;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using VGManager.AzureAdapter.Interfaces;
using VGManager.AzureAdapter;
using VGManager.Services.Interfaces;
using VGManager.Services.VariableGroupServices;
using VGManager.Services;
using VGManager.Api;

static partial class Program
{
    public static WebApplicationBuilder ConfigureServices(WebApplicationBuilder self, string myAllowSpecificOrigins)
    {
        var configuration = self.Configuration;
        var services = self.Services;

        services.AddCors(options =>
        {
            options.AddPolicy(name: myAllowSpecificOrigins,
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

        services.AddHealthChecks()
            .AddCheck<StartupHealthCheck>(nameof(StartupHealthCheck), tags: new[] { "startup" });

        services.AddAutoMapper(
            typeof(Program),
            typeof(VariableGroupProfile),
            typeof(ServiceProfiles.ProjectProfile)
        );

        RegisterServices(services, configuration);

        return self;
    }

    private static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<StartupHealthCheck>();
        AddDatabase<OperationDbContext>(
            services,
            configuration,
            new DatabaseConfiguration
            {
                ProviderKey = "DatabaseProvider",
                PostgreConnectionStringKey = Constants.ConnectionStringKeys.PostgreSql,
                PostgreMigrationsAssemblyKey = Constants.MigrationAssemblyNames.PostgreSql,
            }
        );

        services.AddScoped<IVariableGroupService, VariableGroupService>();
        services.AddScoped<IKeyVaultService, KeyVaultService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IVariableGroupAdapter, VariableGroupAdapter>();
        services.AddScoped<IProjectAdapter, ProjectAdapter>();
        services.AddScoped<IKeyVaultAdapter, KeyVaultAdapter>();
    }

    private static void AddDatabase<TDbContext>(
        IServiceCollection services, 
        IConfiguration configuration, 
        DatabaseConfiguration databaseConfiguration, 
        ServiceLifetime scope = ServiceLifetime.Scoped
        ) where TDbContext : DbContext
    {
        var configuration2 = configuration;
        var databaseConfiguration2 = databaseConfiguration;
        services.AddDbContext<TDbContext>(delegate (DbContextOptionsBuilder options)
        {
            options.UseNpgsql(configuration2.GetConnectionString(databaseConfiguration2.PostgreConnectionStringKey), delegate (NpgsqlDbContextOptionsBuilder options)
            {
                options.MigrationsAssembly(databaseConfiguration2.PostgreMigrationsAssemblyKey);
            });
        }, scope);
    }
}
