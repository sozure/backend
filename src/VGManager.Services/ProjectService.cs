using Microsoft.Extensions.Options;
using Microsoft.TeamFoundation.Core.WebApi;
using VGManager.Repository.Interfaces;
using VGManager.Services.Interfaces;
using VGManager.Services.Models;
using VGManager.Services.Settings;

namespace VGManager.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly ProjectSettings _projectSettings;

    public ProjectService(IProjectRepository projectRepository, IOptions<ProjectSettings> projectSettings)
    {
        _projectRepository = projectRepository;
        _projectSettings = projectSettings.Value;
    }

    public async Task<IEnumerable<TeamProjectReference>> GetProjects(ProjectModel projectModel, CancellationToken cancellationToken = default)
    {
        var url = $"{_projectSettings.BaseUrl}/{projectModel.Organization}";
        return await _projectRepository.GetProjects(url, projectModel.PAT, cancellationToken);
    }
}
