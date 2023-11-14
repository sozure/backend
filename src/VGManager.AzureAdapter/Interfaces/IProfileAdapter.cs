using Microsoft.VisualStudio.Services.Profile;

namespace VGManager.AzureAdapter.Interfaces;

public interface IProfileAdapter
{
    void Setup(string organization, string pat);
    Task<Profile?> GetProfileAsync(CancellationToken cancellationToken = default);
}
