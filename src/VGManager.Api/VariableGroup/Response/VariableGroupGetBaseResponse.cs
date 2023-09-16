namespace VGManager.Api.VariableGroup.Response;

public abstract class VariableGroupGetBaseResponse
{
    public string Project { get; set; } = null!;
    public bool SecretVariableGroup { get; set; }
    public string VariableGroupName { get; set; } = null!;
    public string VariableGroupKey { get; set; } = null!;
}
