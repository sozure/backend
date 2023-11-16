namespace VGManager.Services.Models.Changes;

public class EditionModel : OperationModel
{
    public string Key { get; set; } = null!;
    public string NewValue { get; set; } = null!;
}
