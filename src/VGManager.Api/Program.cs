namespace VGManager.Api;

static partial class Program
{
    public async static Task Main(string[] args)
    {
        var specificOrigins = Constants.Cors.AllowSpecificOrigins;

        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.ClearProviders();
        builder.Logging.AddSimpleConsole();

        ConfigureServices(builder, specificOrigins);

        var app = builder.Build();

        Configure(app, specificOrigins);
        await app.RunAsync();
    }
}


