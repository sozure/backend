using AutoMapper;
using VGManager.Api.Common;
using VGManager.Api.Endpoints.Project.Response;
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
