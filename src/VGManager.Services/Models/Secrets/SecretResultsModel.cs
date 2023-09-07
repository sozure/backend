using VGManager.AzureAdapter.Entities;

namespace VGManager.Services.Models.Secrets;
public class SecretResultsModel
{
    public Status Status { get; set; }
    public IEnumerable<SecretResultModel> Secrets { get; set; } = null!;
}
