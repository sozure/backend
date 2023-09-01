using Microsoft.TeamFoundation.Core.WebApi;
using VGManager.Repository.Interfaces;
using VGManager.Services.Interfaces;
using VGManager.Services.Models;

namespace VGManager.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<IEnumerable<TeamProjectReference>> GetProjects(ProjectModel projectModel, CancellationToken cancellationToken = default)
    {
        return await _projectRepository.GetProjects(projectModel.BaseUrl, projectModel.PAT, cancellationToken);
    }
}
