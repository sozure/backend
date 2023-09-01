namespace VGManager.Services.Models.VariableGroups;

public class VariableGroupModel
{
    public string Organization { get; set; } = null!;

    public string Project { get; set; } = null!;

    public string PAT { get; set; } = null!;

    public string VariableGroupFilter { get; set; } = null!;

    public string KeyFilter { get; set; } = null!;

    public string? ValueFilter { get; set; }
}
