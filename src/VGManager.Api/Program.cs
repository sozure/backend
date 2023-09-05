using VGManager.Api;
using VGManager.Api.MapperProfiles;
using VGManager.Repository;
using VGManager.Repository.Interfaces;
using VGManager.Services;
using VGManager.Services.Interfaces;
using ServiceProfiles = VGManager.Services.MapperProfiles;
using VGManager.Services.Settings;
using System.Reflection;
using Serilog;

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

var assembly = Assembly.GetExecutingAssembly();
var assemblyName = assembly.GetName();
var assemblyInformationalVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();

try
{
    var builder = WebApplication.CreateBuilder(args);

    Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();

    Log.Information("Starting up {AssemblyName}", assemblyName.Name);
    Log.Information("Assembly version {AssemblyVersion}", assemblyName.Version);
    Log.Information("Version {AssemblyInformationalVersion}", assemblyInformationalVersion?.InformationalVersion);

    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(Log.Logger);

    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: myAllowSpecificOrigins,
                          policy =>
                          {
                              policy.AllowAnyMethod();
                              policy.AllowAnyHeader();
                              policy.WithOrigins("http://localhost:3000");
                          });
    });

    builder.Services.AddControllers();
    builder.Services.AddScoped<IVariableGroupService, VariableGroupService>();
    builder.Services.AddScoped<IKeyVaultService, KeyVaultService>();
    builder.Services.AddScoped<IProjectService, ProjectService>();
    builder.Services.AddScoped<IVariableGroupConnectionRepository, VariableGroupConnectionRepository>();
    builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
    builder.Services.AddScoped<IKeyVaultConnectionRepository, KeyVaultConnectionRepository>();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddHealthChecks();
    builder.Services.AddAutoMapper(typeof(Program), typeof(VariableGroupProfile), typeof(ServiceProfiles.ProjectProfile));

    builder.Services.AddOptions<ProjectSettings>()
                .Bind(builder.Configuration.GetSection(Constants.SettingsKey.ProjectSettings))
                .ValidateDataAnnotations();

    var app = builder.Build();

    app.MapHealthChecks("/health");

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseCors(myAllowSpecificOrigins);

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
} 
catch(Exception ex)
{
    Log.Fatal(
        ex,
        "Failed to run {AssemblyName} {AssemblyInformationalVersion}",
        assemblyName.Name,
        assemblyInformationalVersion?.InformationalVersion
    );
}
finally
{
    Log.CloseAndFlush();
}

