using System.Reflection;
using VGManager.Api;
using VGManager.Api.MapperProfiles;
using VGManager.AzureAdapter;
using VGManager.AzureAdapter.Interfaces;
using VGManager.Services;
using VGManager.Services.Interfaces;
using VGManager.Services.VariableGroupServices;
using ServiceProfiles = VGManager.Services.MapperProfiles;

var myAllowSpecificOrigins = Constants.Cors.AllowSpecificOrigins;

var assembly = Assembly.GetExecutingAssembly();
var assemblyName = assembly.GetName();
var assemblyInformationalVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: myAllowSpecificOrigins,
                          policy =>
                          {
                              policy.WithOrigins("http://localhost:3000")
                                .AllowAnyMethod()
                                .AllowAnyHeader();
                          });
    });

    builder.Services.AddControllers();
    builder.Services.AddScoped<IVariableGroupService, VariableGroupService>();
    builder.Services.AddScoped<IKeyVaultService, KeyVaultService>();
    builder.Services.AddScoped<IProjectService, ProjectService>();
    builder.Services.AddScoped<IVariableGroupAdapter, VariableGroupAdapter>();
    builder.Services.AddScoped<IProjectAdapter, ProjectAdapter>();
    builder.Services.AddScoped<IKeyVaultAdapter, KeyVaultAdapter>();

    builder.Services.AddLogging(configure => configure.AddConsole());

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddHealthChecks();
    builder.Services.AddAutoMapper(typeof(Program), typeof(VariableGroupProfile), typeof(ServiceProfiles.ProjectProfile));

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
catch (Exception ex)
{

}
finally
{
}

