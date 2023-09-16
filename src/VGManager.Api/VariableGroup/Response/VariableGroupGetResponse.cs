using VGManager.Api.VariableGroup.Response;

namespace VGManager.Api.VariableGroups.Response;

public class VariableGroupGetResponse: VariableGroupGetBaseResponse
{
    public string VariableGroupValue { get; set; } = null!;
}
