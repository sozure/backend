using Microsoft.VisualStudio.Services.Profile;
using VGManager.AzureAdapter.Interfaces;
using VGManager.Services.Interfaces;

namespace VGManager.Services;

public class ProfileService: IProfileService
{
    private readonly IProfileAdapter _profileAdapter;

    public ProfileService(IProfileAdapter profileAdapter)
    {
        _profileAdapter = profileAdapter;
    }

    public async Task<Profile?> GetProfile(string organization, string pat, CancellationToken cancellationToken = default)
    {
        _profileAdapter.Setup(organization, pat);
        return await _profileAdapter.GetProfile(cancellationToken);
    }
}
