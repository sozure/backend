using AutoMapper;
using VGManager.Api.Common;
using VGManager.Api.Projects.Responses;
using VGManager.Services.Models.Common;
using VGManager.Services.Models.Projects;

namespace VGManager.Api.MapperProfiles;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<ProjectResult, ProjectResponse>()
            .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Project.Name));
        CreateMap<BasicRequest, BaseModel>();
    }
}
