using AutoMapper;
using Microsoft.TeamFoundation.Core.WebApi;
using VGManager.Api.Projects;
using VGManager.Api.Projects.Responses;
using VGManager.Services.Models.Projects;

namespace VGManager.Api.MapperProfiles;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<ProjectResultModel, ProjectsResponse>();
        CreateMap<TeamProjectReference, ProjectResponse>()
            .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name));
        CreateMap<ProjectRequest, ProjectModel>();
    }
}
