using Microsoft.VisualStudio.Services.Profile;

namespace VGManager.Services.Interfaces;

public interface IProfileService
{
    Task<Profile?> GetProfile(string organization, string pat, CancellationToken cancellationToken = default);
}
