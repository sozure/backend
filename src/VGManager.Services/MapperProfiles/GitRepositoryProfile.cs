using AutoMapper;
using VGManager.Services.Models;
using VGManager.Services.Models.GitRepositories;

namespace VGManager.Services.MapperProfiles;

public class GitRepositoryProfile : Profile
{
    public GitRepositoryProfile()
    {
        CreateMap<GitRepositoryModel, GitRepositoryEntity>();
    }
}
