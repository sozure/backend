namespace VGManager.Services.Models.VariableGroups.Results;

public class VariableGroupResult
{
    public string Project { get; set; } = null!;
    public bool SecretVariableGroup { get; set; }
    public string VariableGroupName { get; set; } = null!;
    public string VariableGroupKey { get; set; } = null!;
    public string? VariableGroupValue { get; set; }
    public string? KeyVaultName { get; set; }
}
