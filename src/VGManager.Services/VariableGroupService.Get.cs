using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.DistributedTask.WebApi;
using System.Text.RegularExpressions;
using VGManager.AzureAdapter.Entities;
using VGManager.Services.Models.VariableGroups.Requests;
using VGManager.Services.Models.VariableGroups.Results;

namespace VGManager.Services;
public partial class VariableGroupService
{
    public async Task<VariableGroupResults> GetVariableGroupsAsync(
        VariableGroupModel variableGroupModel,
        CancellationToken cancellationToken = default
        )
    {
        var matchedVariableGroups = new List<VariableGroupResult>();
        var vgEntity = await _variableGroupConnectionRepository.GetAllAsync(cancellationToken);
        var status = vgEntity.Status;

        if (status == Status.Success)
        {
            var filteredVariableGroups = variableGroupModel.ContainsSecrets ?
                Filter(vgEntity.VariableGroups, variableGroupModel.VariableGroupFilter) :
                FilterWithoutSecrets(vgEntity.VariableGroups, variableGroupModel.VariableGroupFilter);

            var valueFilter = variableGroupModel.ValueFilter;
            Regex? regex = null;

            if (valueFilter is not null)
            {
                try
                {
                    regex = new Regex(valueFilter.ToLower());
                }
                catch (RegexParseException ex)
                {
                    _logger.LogError(ex, "Couldn't parse and create regex. Value: {value}.", valueFilter);
                    return new()
                    {
                        Status = status,
                        VariableGroups = matchedVariableGroups,
                    };
                }
            }

            foreach (var filteredVariableGroup in filteredVariableGroups)
            {
                matchedVariableGroups.AddRange(
                    GetVariables(variableGroupModel.KeyFilter, regex, filteredVariableGroup)
                    );
            }

            return new()
            {
                Status = status,
                VariableGroups = matchedVariableGroups,
            };
        }
        else
        {
            return new()
            {
                Status = status,
                VariableGroups = new List<VariableGroupResult>(),
            };
        }
    }

    private List<VariableGroupResult> GetVariables(
        string keyFilter,
        Regex? regex,
        VariableGroup filteredVariableGroup
        )
    {
        var result = new List<VariableGroupResult>();
        var filteredVariables = Filter(filteredVariableGroup.Variables, keyFilter);

        foreach (var filteredVariable in filteredVariables)
        {
            var variableValue = filteredVariable.Value.Value ?? string.Empty;
            if (regex is not null)
            {
                if (regex.IsMatch(variableValue.ToLower()))
                {
                    AddVariableGroupResult(filteredVariableGroup, result, filteredVariable, variableValue);
                }
            }
            else
            {
                AddVariableGroupResult(filteredVariableGroup, result, filteredVariable, variableValue);
            }
        }
        return result;
    }
}
