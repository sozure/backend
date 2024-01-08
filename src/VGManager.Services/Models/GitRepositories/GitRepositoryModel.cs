using VGManager.Services.Models.Common;

namespace VGManager.Services.Models.GitRepositories;

public class GitRepositoryModel: BaseModel
{
    public string Project { get; set; } = null!;

    public string RepositoryId { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public string Delimiter { get; set; } = null!;

    public string Branch { get; set; } = null!;

    public IEnumerable<string>? Exceptions { get; set; } = null!;
}
