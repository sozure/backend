namespace VGManager.Models;

public class RepositoryResponseModel<T>
{
    public RepositoryStatus Status { get; set; }
    public IEnumerable<T> Data { get; set; } = null!;
}
