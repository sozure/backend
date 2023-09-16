using VGManager.AzureAdapter.Entities;

namespace VGManager.Services.Models.Secrets.Results;
public class SecretResults
{
    public Status Status { get; set; }
    public IEnumerable<SecretResult> Secrets { get; set; } = null!;
}
