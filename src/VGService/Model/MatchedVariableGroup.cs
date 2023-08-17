namespace VGService.Model;

public class MatchedVariableGroup
{
    public string VariableGroupName { get; set; }
    public string VariableGroupKey { get; set; }
    public string VariableGroupValue { get; set; }

    public MatchedVariableGroup(string variableGroupName, string variableGroupKey, string variableGroupValue)
    {
        VariableGroupName = variableGroupName;
        VariableGroupKey = variableGroupKey;
        VariableGroupValue = variableGroupValue;
    }
}
