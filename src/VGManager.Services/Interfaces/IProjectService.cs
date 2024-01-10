using VGManager.Models.Models;
using VGManager.Services.Models.Common;
using VGManager.Services.Models.Projects;

namespace VGManager.Services.Interfaces;
public interface IProjectService
{
    Task<AdapterResponseModel<IEnumerable<ProjectResult>>> GetProjectsAsync(BaseModel projectModel, CancellationToken cancellationToken = default);
}
