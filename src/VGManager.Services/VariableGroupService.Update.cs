using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.DistributedTask.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VGManager.AzureAdapter.Entities;
using VGManager.Services.Models.VariableGroups.Requests;

namespace VGManager.Services;
public partial class VariableGroupService
{
    public async Task<Status> UpdateVariableGroupsAsync(
        VariableGroupUpdateModel variableGroupUpdateModel,
        CancellationToken cancellationToken = default
        )
    {
        var vgEntity = await _variableGroupConnectionRepository.GetAllAsync(cancellationToken);
        var status = vgEntity.Status;

        if (status == Status.Success)
        {
            var filteredVariableGroups = FilterWithoutSecrets(vgEntity.VariableGroups, variableGroupUpdateModel.VariableGroupFilter);
            var valueFilter = variableGroupUpdateModel.ValueFilter;
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
                    return status;
                }
            }

            return await UpdateVariableGroupsAsync(variableGroupUpdateModel, filteredVariableGroups, regex, cancellationToken);
        }
        return status;
    }

    private async Task<Status> UpdateVariableGroupsAsync(VariableGroupUpdateModel variableGroupUpdateModel, IEnumerable<VariableGroup> filteredVariableGroups, Regex? regex, CancellationToken cancellationToken)
    {
        var updateCounter1 = 0;
        var updateCounter2 = 0;
        foreach (var filteredVariableGroup in filteredVariableGroups)
        {
            var variableGroupName = filteredVariableGroup.Name;
            var updateIsNeeded = UpdateVariables(variableGroupUpdateModel, regex, filteredVariableGroup);

            if (updateIsNeeded)
            {
                updateCounter2++;
                var variableGroupParameters = GetVariableGroupParameters(filteredVariableGroup, variableGroupName);

                var updateStatus = await _variableGroupConnectionRepository.UpdateAsync(
                    variableGroupParameters,
                    filteredVariableGroup.Id,
                    cancellationToken
                    );

                if (updateStatus == Status.Success)
                {
                    updateCounter1++;
                    _logger.LogDebug("{variableGroupName} updated.", variableGroupName);
                }
            }
        }
        return updateCounter1 == updateCounter2 ? Status.Success : Status.Unknown;
    }

    private bool UpdateVariables(
        VariableGroupUpdateModel variableGroupUpdateModel,
        Regex? regex,
        VariableGroup filteredVariableGroup
        )
    {
        var filteredVariables = Filter(filteredVariableGroup.Variables, variableGroupUpdateModel.KeyFilter);
        var updateIsNeeded = false;

        foreach (var filteredVariable in filteredVariables)
        {
            var variableValue = filteredVariable.Value.Value;
            var newValue = variableGroupUpdateModel.NewValue;

            if (regex is not null)
            {
                if (regex.IsMatch(variableValue))
                {
                    filteredVariable.Value.Value = newValue;
                    updateIsNeeded = true;
                }
            }
            else
            {
                filteredVariable.Value.Value = newValue;
                updateIsNeeded = true;
            }
        }

        return updateIsNeeded;
    }
}
