using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.DistributedTask.WebApi;
using System.Text.RegularExpressions;
using VGManager.AzureAdapter.Entities;
using VGManager.Services.Models.VariableGroups.Requests;

namespace VGManager.Services.VariableGroupServices;
public partial class VariableGroupService
{
    public async Task<Status> DeleteVariablesAsync(VariableGroupModel variableGroupModel, CancellationToken cancellationToken = default)
    {
        var vgEntity = await _variableGroupConnectionRepository.GetAllAsync(cancellationToken);
        var status = vgEntity.Status;

        if (status == Status.Success)
        {
            var filteredVariableGroups = FilterWithoutSecrets(vgEntity.VariableGroups, variableGroupModel.VariableGroupFilter);
            return await DeleteVariablesAsync(variableGroupModel, filteredVariableGroups, cancellationToken);
        }

        return status;
    }

    private async Task<Status> DeleteVariablesAsync(
        VariableGroupModel variableGroupModel, 
        IEnumerable<VariableGroup> filteredVariableGroups, 
        CancellationToken cancellationToken
        )
    {
        var deletionCounter1 = 0;
        var deletionCounter2 = 0;
        var filter = variableGroupModel.KeyFilter;
        Regex regex;
        try
        {
            regex = new Regex(filter.ToLower());
        }
        catch (RegexParseException ex)
        {
            _logger.LogError(ex, "Couldn't parse and create regex. Value: {value}.", filter);
            return Status.Success;
        }

        foreach (var filteredVariableGroup in filteredVariableGroups)
        {
            var variableGroupName = filteredVariableGroup.Name;

            var deleteIsNeeded = DeleteVariables(
                filteredVariableGroup,
                variableGroupModel.ValueFilter,
                regex
                );

            if (deleteIsNeeded)
            {
                deletionCounter1++;
                var variableGroupParameters = GetVariableGroupParameters(filteredVariableGroup, variableGroupName);

                var updateStatus = await _variableGroupConnectionRepository.UpdateAsync(
                    variableGroupParameters,
                    filteredVariableGroup.Id,
                    cancellationToken
                    );

                if (updateStatus == Status.Success)
                {
                    deletionCounter2++;
                }
            }
        }
        return deletionCounter1 == deletionCounter2 ? Status.Success : Status.Unknown;
    }

    private static bool DeleteVariables(VariableGroup filteredVariableGroup, string? valueCondition, Regex regex)
    {
        var deleteIsNeeded = false;
        var filteredVariables = Filter(filteredVariableGroup.Variables, regex);
        foreach (var filteredVariable in filteredVariables)
        {
            var variableValue = filteredVariable.Value.Value;

            if (valueCondition is not null)
            {
                if (valueCondition.Equals(variableValue))
                {
                    filteredVariableGroup.Variables.Remove(filteredVariable.Key);
                    deleteIsNeeded = true;
                }
            }
            else
            {
                filteredVariableGroup.Variables.Remove(filteredVariable.Key);
                deleteIsNeeded = true;
            }
        }

        return deleteIsNeeded;
    }
}
