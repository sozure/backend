using AutoMapper;
using VGManager.Api.GitRepository.Request;
using VGManager.Api.GitRepository.Response;
using VGManager.Services.Models.GitRepositories;

namespace VGManager.Api.MapperProfiles;

public class GitRepositoryProfile : Profile
{
    public GitRepositoryProfile()
    {
        CreateMap<GitRepositoryResult, GitRepositoryResponse>();
        CreateMap<GitRepositoryResults, GitRepositoryResponses>();
        CreateMap<GitRepositoryVariablesRequest, GitRepositoryModel>();
        CreateMap<GitRepositoryVariablesResult, GitRepositoryVariablesResponse>();
    }
}
