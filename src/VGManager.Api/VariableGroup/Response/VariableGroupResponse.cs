
namespace VGManager.Api.VariableGroups.Response;

public class VariableGroupResponse
{
    public string Project { get; set; } = null!;
    public bool SecretVariableGroup { get; set; }
    public string VariableGroupName { get; set; } = null!;
    public string VariableGroupKey { get; set; } = null!;
    public string? VariableGroupValue { get; set; }
    public string? KeyVaultName { get; set; }
}
