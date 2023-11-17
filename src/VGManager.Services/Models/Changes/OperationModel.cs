namespace VGManager.Services.Models.Changes;

public class OperationModel
{
    public string Id { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string User { get; set; } = null!;
    public string Organization { get; set; } = null!;
    public string Project { get; set; } = null!;
    public DateTime Date { get; set; }
    public string VariableGroupFilter { get; set; } = null!;
}
