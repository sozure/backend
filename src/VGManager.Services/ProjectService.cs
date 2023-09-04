using AutoMapper;
using Microsoft.Extensions.Options;
using VGManager.Repository.Interfaces;
using VGManager.Services.Interfaces;
using VGManager.Services.Models.Projects;
using VGManager.Services.Settings;

namespace VGManager.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly ProjectSettings _projectSettings;
    private readonly IMapper _mapper;

    public ProjectService(IProjectRepository projectRepository, IMapper mapper, IOptions<ProjectSettings> projectSettings)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
        _projectSettings = projectSettings.Value;
    }

    public async Task<ProjectResultModel> GetProjects(ProjectModel projectModel, CancellationToken cancellationToken = default)
    {
        var url = $"{_projectSettings.BaseUrl}/{projectModel.Organization}";
        var projectEntity = await _projectRepository.GetProjects(url, projectModel.PAT, cancellationToken);
        return _mapper.Map<ProjectResultModel>(projectEntity);
    }
}
