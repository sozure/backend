using AutoMapper;
using VGManager.Api.GitRepository;
using VGManager.Services.Models.GitRepositories;

namespace VGManager.Api.MapperProfiles;

public class GitRepositoryProfile : Profile
{
    public GitRepositoryProfile()
    {
        CreateMap<GitRepositoryResult, GitRepositoryResponse>();
    }
}
