namespace VGManager.Models;

public class AdapterResponseModel<T>
{
    public AdapterStatus Status { get; set; }
    public T Data { get; set; } = default!;
}
