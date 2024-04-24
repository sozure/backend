using AutoMapper;
using VGManager.Api.Endpoints.GitVersion;
using VGManager.Services.Models;

namespace VGManager.Api.MapperProfiles;

public class GitVersionProfile : Profile
{
    public GitVersionProfile()
    {
        CreateMap<GitLatestTagsRequest, GitLatestTagsEntity>();
    }
}
