using Microsoft.OpenApi.Models;
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

builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "VGManager.Api", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    c.UseOneOfForPolymorphism();
});

builder.Services.AddHealthChecks();
builder.Services.AddAutoMapper(
    typeof(Program), 
    typeof(VariableGroupProfile), 
    typeof(ServiceProfiles.ProjectProfile)
    );

var app = builder.Build();

app.MapHealthChecks("/health");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors(myAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
