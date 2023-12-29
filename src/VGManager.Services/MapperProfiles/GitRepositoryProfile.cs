using AutoMapper;
using VGManager.AzureAdapter.Entities;
using VGManager.Services.Models.GitRepositories;

namespace VGManager.Services.MapperProfiles;

public class GitRepositoryProfile: Profile
{
    public GitRepositoryProfile()
    {
        CreateMap<GitRepositoryModel, GitRepositoryEntity>();
    }
}
