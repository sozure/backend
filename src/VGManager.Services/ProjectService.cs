using Microsoft.TeamFoundation.Core.WebApi;
using VGManager.Repository.Interfaces;
using VGManager.Services.Interfaces;
using VGManager.Services.Models;

namespace VGManager.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly string BaseUrl = "https://dev.azure.com";

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<IEnumerable<TeamProjectReference>> GetProjects(ProjectModel projectModel, CancellationToken cancellationToken = default)
    {
        var url = $"{BaseUrl}/{projectModel.Organization}";
        return await _projectRepository.GetProjects(url, projectModel.PAT, cancellationToken);
    }
}
