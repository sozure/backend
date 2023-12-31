using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.DistributedTask.WebApi;
using System.Text.RegularExpressions;
using VGManager.AzureAdapter.Entities;
using VGManager.AzureAdapter.Interfaces;
using VGManager.Services.Interfaces;
using VGManager.Services.Models.VariableGroups.Requests;
using VGManager.Services.Models.VariableGroups.Results;

namespace VGManager.Services;

public class VariableGroupService: IVariableGroupService
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

    public async Task<VariableGroupResults> GetVariableGroupsAsync(
        VariableGroupModel variableGroupModel,
        bool containsKey,
        CancellationToken cancellationToken = default
        )
    {
        var result = new List<VariableGroup>();
        var vgEntity = await _variableGroupConnectionRepository.GetAllAsync(cancellationToken);
        var status = vgEntity.Status;

        if (status == AdapterStatus.Success)
        {
            var filteredVariableGroups = variableGroupModel.ContainsSecrets ?
                        _variableFilterService.Filter(vgEntity.VariableGroups, variableGroupModel.VariableGroupFilter) :
                        _variableFilterService.FilterWithoutSecrets(true, variableGroupModel.VariableGroupFilter, vgEntity.VariableGroups);
            foreach (var variableGroup in filteredVariableGroups)
            {
                if (containsKey)
                {
                    if (variableGroup.Variables.ContainsKey(variableGroupModel.KeyFilter))
                    {
                        result.Add(variableGroup);
                    }
                } else
                {
                    if (!variableGroup.Variables.ContainsKey(variableGroupModel.KeyFilter))
                    {
                        result.Add(variableGroup);
                    }
                }
            }

            return new()
            {
                Status = status,
                VariableGroups = result,
            };
        }
        else
        {
            return new()
            {
                Status = status,
                VariableGroups = Enumerable.Empty<VariableGroup>(),
            };
        }
    }

    
}
