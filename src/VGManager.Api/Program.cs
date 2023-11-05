using VGManager.Api;

var myAllowSpecificOrigins = Constants.Cors.AllowSpecificOrigins;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole();

ConfigureServices(builder, myAllowSpecificOrigins);

var app = builder.Build();

await ConfigureAsync(app, myAllowSpecificOrigins);
await app.RunAsync();
