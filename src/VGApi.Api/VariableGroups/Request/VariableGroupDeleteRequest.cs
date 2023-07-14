namespace VGApi.Api.VariableGroups.Request;

public class VariableGroupDeleteRequest
{
    public string Organization { get; set; } = null!;
    public string Project { get; set; } = null!;
    public string Pat { get; set; } = null!;
    public string VariableGroupFilter { get; set; } = null!;
    public string KeyFilter { get; set; } = null!;
    public string? ValueFilter { get; set; }
}
