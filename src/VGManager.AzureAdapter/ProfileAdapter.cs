using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.Profile;
using Microsoft.VisualStudio.Services.Profile.Client;
using Microsoft.VisualStudio.Services.WebApi;
using VGManager.AzureAdapter.Interfaces;

namespace VGManager.AzureAdapter;

public class ProfileAdapter: IProfileAdapter
{
    private VssConnection _connection = null!;
    private readonly ILogger _logger;

    public ProfileAdapter(ILogger<VariableGroupAdapter> logger)
    {
        _logger = logger;
    }

    public void Setup(string organization, string pat)
    {
        var uriString = $"https://dev.azure.com/{organization}";
        Uri uri;
        Uri.TryCreate(uriString, UriKind.Absolute, out uri!);

        var credentials = new VssBasicCredential(string.Empty, pat);
        _connection = new VssConnection(uri, credentials);
    }

    public async Task<Profile?> GetProfile(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Request profile from Azure DevOps.");
        var client = await _connection.GetClientAsync<ProfileHttpClient>(cancellationToken);
        var profileQueryContext = new ProfileQueryContext(AttributesScope.Core);

        try
        {
            return await client.GetProfileAsync(profileQueryContext, cancellationToken, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Couldn't get profile.");
            return null!;
        }
    }
}
