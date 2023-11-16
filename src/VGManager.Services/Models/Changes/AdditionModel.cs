namespace VGManager.Services.Models.Changes;

public class AdditionModel : OperationModel
{
    public string Key { get; set; } = null!;
    public string Value { get; set; } = null!;
}
