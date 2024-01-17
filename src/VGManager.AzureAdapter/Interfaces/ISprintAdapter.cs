using VGManager.Models.StatusEnums;

namespace VGManager.AzureAdapter.Interfaces;

public interface ISprintAdapter
{
    Task<(AdapterStatus, string)> GetCurrentSprintAsync(string project, CancellationToken cancellationToken = default);
}
