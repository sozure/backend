using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.DistributedTask.WebApi;
using VGManager.AzureAdapter.Interfaces;
using VGManager.Models;
using VGManager.Services.Interfaces;
using VGManager.Services.Models.VariableGroups.Requests;
using VGManager.Services.Models.VariableGroups.Results;

namespace VGManager.Services;

public class VariableGroupService : IVariableGroupService
{
    private readonly IVariableFilterService _variableFilterService;
    private readonly IVariableGroupAdapter _variableGroupConnectionRepository;
    private readonly ILogger _logger;

    public VariableGroupService(
        IVariableFilterService variableFilterService,
        IVariableGroupAdapter variableGroupConnectionRepository,
        ILogger<VariableGroupService> logger
        )
    {
        _variableFilterService = variableFilterService;
        _variableGroupConnectionRepository = variableGroupConnectionRepository;
        _logger = logger;
    }

    public async Task<AdapterResponseModel<IEnumerable<VariableGroup>>> GetVariableGroupsAsync(
        VariableGroupModel variableGroupModel,
        bool containsKey,
        CancellationToken cancellationToken = default
        )
    {
        _logger.LogInformation("Get variable groups from {project} Azure project.", variableGroupModel.Project);
        var vgEntity = await _variableGroupConnectionRepository.GetAllAsync(cancellationToken);
        var status = vgEntity.Status;

        if (status == AdapterStatus.Success)
        {
            var filteredVariableGroups = variableGroupModel.ContainsSecrets ?
                        _variableFilterService.Filter(vgEntity.VariableGroups, variableGroupModel.VariableGroupFilter) :
                        _variableFilterService.FilterWithoutSecrets(true, variableGroupModel.VariableGroupFilter, vgEntity.VariableGroups);

            var result = GetVariableGroups(filteredVariableGroups, variableGroupModel.KeyFilter, containsKey);
            return GetResult(status, result);
        }
        else
        {
            return GetResult(status, Enumerable.Empty<VariableGroup>());
        }
    }

    private static IEnumerable<VariableGroup> GetVariableGroups(IEnumerable<VariableGroup> filteredVariableGroups, string keyFilter, bool containsKey)
    {
        var result = new List<VariableGroup>();
        foreach (var variableGroup in filteredVariableGroups)
        {
            if (containsKey)
            {
                if (variableGroup.Variables.ContainsKey(keyFilter))
                {
                    result.Add(variableGroup);
                }
            }
            else
            {
                if (!variableGroup.Variables.ContainsKey(keyFilter))
                {
                    result.Add(variableGroup);
                }
            }
        }
        return result;
    }

    private static AdapterResponseModel<IEnumerable<VariableGroup>> GetResult(AdapterStatus status, IEnumerable<VariableGroup> variableGroups)
        => new()
        {
            Status = status,
            Data = variableGroups
        };
}
