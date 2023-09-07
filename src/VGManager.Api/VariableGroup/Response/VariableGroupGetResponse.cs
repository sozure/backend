namespace VGManager.Api.VariableGroups.Response;

public class VariableGroupGetResponse
{
    public string Project { get; set; } = null!;
    public string VariableGroupName { get; set; } = null!;
    public string VariableGroupKey { get; set; } = null!;
    public string VariableGroupValue { get; set; } = null!;
}
