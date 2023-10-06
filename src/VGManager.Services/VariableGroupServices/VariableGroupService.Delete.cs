using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.DistributedTask.WebApi;
using System.Text.RegularExpressions;
using VGManager.AzureAdapter.Entities;
using VGManager.Services.Models.VariableGroups.Requests;

namespace VGManager.Services.VariableGroupServices;
public partial class VariableGroupService
{
    public async Task<Status> DeleteVariablesAsync(
        VariableGroupModel variableGroupModel,
        bool filterAsRegex,
        CancellationToken cancellationToken = default
        )
    {
        var vgEntity = await _variableGroupConnectionRepository.GetAllAsync(cancellationToken);
        var status = vgEntity.Status;

        if (status == Status.Success)
        {
            var filteredVariableGroups = FilterWithoutSecrets(filterAsRegex, variableGroupModel.VariableGroupFilter, vgEntity.VariableGroups);
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
        var keyFilter = variableGroupModel.KeyFilter;

        foreach (var filteredVariableGroup in filteredVariableGroups)
        {
            var variableGroupName = filteredVariableGroup.Name;

            var deleteIsNeeded = DeleteVariables(
                filteredVariableGroup,
                keyFilter,
                variableGroupModel.ValueFilter
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

    private static bool DeleteVariables(VariableGroup filteredVariableGroup, string keyFilter, string? valueCondition)
    {
        var deleteIsNeeded = false;
        var filteredVariables = Filter(filteredVariableGroup.Variables, keyFilter);
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
