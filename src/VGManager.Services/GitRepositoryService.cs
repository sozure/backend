using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Services.Common;
using VGManager.AzureAdapter.Entities;
using VGManager.AzureAdapter.Interfaces;
using VGManager.Models.Models;
using VGManager.Models.StatusEnums;
using VGManager.Services.Interfaces;
using VGManager.Services.Models.GitRepositories;

namespace VGManager.Services;

public class GitRepositoryService : IGitRepositoryService
{
    private readonly IGitRepositoryAdapter _gitRepositoryAdapter;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public GitRepositoryService(IGitRepositoryAdapter gitRepositoryAdapter, IMapper mapper, ILogger<GitRepositoryService> logger)
    {
        _gitRepositoryAdapter = gitRepositoryAdapter;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<AdapterResponseModel<IEnumerable<GitRepositoryResult>>> GetAllAsync(
        string organization,
        string project,
        string pat,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var repositoryNames = new List<GitRepositoryResult>();
            var repositories = await _gitRepositoryAdapter.GetAllAsync(organization, project, pat, cancellationToken);

            foreach (var repository in repositories)
            {
                repositoryNames.Add(new()
                {
                    RepositoryId = repository.Id.ToString(),
                    RepositoryName = repository.Name
                });
            }

            return new()
            {
                Data = repositoryNames,
                Status = AdapterStatus.Success
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting git repositories from {project} azure project.", project);
            return new()
            {
                Data = Enumerable.Empty<GitRepositoryResult>(),
                Status = AdapterStatus.Unknown
            };
        }
    }

    public async Task<AdapterResponseModel<IEnumerable<string>>> GetVariablesFromConfigAsync(
        GitRepositoryModel gitRepositoryModel,
        CancellationToken cancellationToken = default
        )
    {
        var pat = gitRepositoryModel.PAT;
        _gitRepositoryAdapter.Setup(gitRepositoryModel.Organization, pat);
        List<string> variables;
        try
        {
            var entity = _mapper.Map<GitRepositoryEntity>(gitRepositoryModel);
            variables = await _gitRepositoryAdapter.GetVariablesFromConfigAsync(
                entity,
                cancellationToken
                );
        }
        catch (VssServiceException ex)
        {
            var status = ex.Message.Contains("could not be found in the repository") ? AdapterStatus.FileDoesNotExists : AdapterStatus.Unknown;
            return new()
            {
                Status = status,
                Data = Enumerable.Empty<string>()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting variables from {project} azure project.", gitRepositoryModel.Project);
            return new()
            {
                Status = AdapterStatus.Unknown,
                Data = Enumerable.Empty<string>()
            };
        }


        return new()
        {
            Status = AdapterStatus.Success,
            Data = variables
        };
    }
}
