using AutoMapper;
using Microsoft.TeamFoundation.Core.WebApi;
using VGManager.Api.Projects;
using VGManager.Services.Models;

namespace VGManager.Api.MapperProfiles;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<TeamProjectReference, ProjectResponse>()
            .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name));
        CreateMap<ProjectRequest, ProjectModel>();
    }
}
