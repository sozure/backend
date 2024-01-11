using AutoMapper;
using VGManager.Api.Endpoints.GitRepository.Request;
using VGManager.Services.Models.GitRepositories;

namespace VGManager.Api.MapperProfiles;

public class GitRepositoryProfile : Profile
{
    public GitRepositoryProfile()
    {
        CreateMap<GitRepositoryVariablesRequest, GitRepositoryModel>();
    }
}
