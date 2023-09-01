using AutoMapper;
using Microsoft.TeamFoundation.Core.WebApi;
using VGManager.Api.Projects;

namespace VGManager.Api.MapperProfiles;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<TeamProjectReference, ProjectResponse>()
            .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name));
    }
}
