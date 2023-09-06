using AutoMapper;
using VGManager.Repository.Entities;
using VGManager.Services.Models.Projects;

namespace VGManager.Services.MapperProfiles;
public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<ProjectEntity, ProjectResultModel>();
    }
}
