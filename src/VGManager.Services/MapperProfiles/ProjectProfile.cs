using AutoMapper;
using VGManager.AzureAdapter.Entities;
using VGManager.Services.Models.Projects;

namespace VGManager.Services.MapperProfiles;
public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<ProjectsEntity, ProjectsResult>();
        CreateMap<ProjectEntity, ProjectResult>();
    }
}
