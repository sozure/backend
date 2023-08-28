using VGManager.Services;
using VGManager.Services.Interfaces;
using VGManager.Services.Repositories;
using VGManager.Services.Repositories.Interface;
using VGManager.Services.Repositories.Interfaces;

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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
builder.Services.AddScoped<IKVService, KVService>();
builder.Services.AddScoped<IVariableGroupConnectionRepository, VariableGroupConnectionRepository>();
builder.Services.AddScoped<IKeyVaultConnectionRepository, KeyVaultConnectionRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks();

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