using Microsoft.VisualStudio.Services.Profile;

namespace VGManager.Services.Interfaces;

public interface IProfileService
{
    Task<Profile?> GetProfileAsync(string organization, string pat, CancellationToken cancellationToken = default);
}
