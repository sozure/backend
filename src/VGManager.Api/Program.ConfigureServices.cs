using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using System.Reflection;
using VGManager.Api;
using VGManager.Api.HealthChecks;
using VGManager.Api.MapperProfiles;
using VGManager.AzureAdapter;
using VGManager.AzureAdapter.Interfaces;
using VGManager.Repositories;
using VGManager.Repositories.Boilerplate;
using VGManager.Repositories.Interfaces;
using VGManager.Repositories.DbContexts;
using VGManager.Services;
using VGManager.Services.Interfaces;
using VGManager.Services.VariableGroupServices;
using ServiceProfiles = VGManager.Services.MapperProfiles;

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

        services.AddAuthorization();
        services.AddControllers();
        services.AddHealthChecks()
            .AddCheck<StartupHealthCheck>(nameof(StartupHealthCheck), tags: new[] { "startup" });

        services.AddAutoMapper(
            typeof(Program),
            typeof(VariableGroupProfile),
            typeof(ServiceProfiles.ProjectProfile)
        );

        //services.AddDbContext<OperationDbContext>(options =>
        //{
        //    UseSqlServer(options, configuration);
        //});

        RegisterServices(services, configuration);

        return self;
    }

    private static void UseSqlServer(DbContextOptionsBuilder options, ConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString("ConnectionStrings__VGManager_API");
        options.UseSqlServer(connectionString);
    }

    private static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<StartupHealthCheck>();

        var connString = configuration.GetConnectionString("VGManager_API");

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

        services.AddScoped<IAdditionColdRepository, AdditionColdRepository>();
        services.AddScoped<IDeletionColdRepository, DeletionColdRepository>();
        services.AddScoped<IEditionColdRepository, EditionColdRepository>();
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
