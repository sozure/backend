using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.DistributedTask.WebApi;
using System.Text.RegularExpressions;
using VGManager.AzureAdapter.Entities;
using VGManager.Services.Models.VariableGroups.Requests;
using VGManager.Services.Models.VariableGroups.Results;

namespace VGManager.Services;
public partial class VariableGroupService
{
    public async Task<Status> AddVariablesAsync(VariableGroupAddModel variableGroupAddModel, CancellationToken cancellationToken = default)
    {
        var vgEntity = await _variableGroupConnectionRepository.GetAllAsync(cancellationToken);
        var status = vgEntity.Status;

        if (status == Status.Success)
        {
            var keyFilter = variableGroupAddModel.KeyFilter;
            var variableGroupFilter = variableGroupAddModel.VariableGroupFilter;
            var key = variableGroupAddModel.Key;
            var value = variableGroupAddModel.Value;
            var filteredVariableGroups = CollectVariableGroups(vgEntity, keyFilter, variableGroupFilter);

            return await AddVariablesAsync(filteredVariableGroups, key, value, cancellationToken);
        }

        return status;
    }

    private void AddVariableGroupResult(VariableGroup filteredVariableGroup, List<VariableGroupResult> result, KeyValuePair<string, VariableValue> filteredVariable, string variableValue)
    {
        if (filteredVariableGroup.Type == "AzureKeyVault")
        {
            var azProviderData = filteredVariableGroup.ProviderData as AzureKeyVaultVariableGroupProviderData;
            result.Add(new VariableGroupResult()
            {
                Project = _project ?? string.Empty,
                SecretVariableGroup = true,
                VariableGroupName = filteredVariableGroup.Name,
                VariableGroupKey = filteredVariable.Key,
                KeyVaultName = azProviderData?.Vault ?? string.Empty
            });
        }
        else
        {
            result.Add(new VariableGroupResult()
            {
                Project = _project ?? string.Empty,
                SecretVariableGroup = false,
                VariableGroupName = filteredVariableGroup.Name,
                VariableGroupKey = filteredVariable.Key,
                VariableGroupValue = variableValue
            });
        }
    }

    private async Task<bool> AddVariableAsync(string key, string value, VariableGroup filteredVariableGroup, CancellationToken cancellationToken)
    {
        var variableGroupName = filteredVariableGroup.Name;
        filteredVariableGroup.Variables.Add(key, value);
        var variableGroupParameters = GetVariableGroupParameters(filteredVariableGroup, variableGroupName);

        var updateStatus = await _variableGroupConnectionRepository.UpdateAsync(
            variableGroupParameters,
            filteredVariableGroup.Id,
            cancellationToken
            );

        if (updateStatus == Status.Success)
        {
            return true;
        }

        return false;
    }

    private IEnumerable<VariableGroup> CollectVariableGroups(VariableGroupEntity vgEntity, string? keyFilter, string variableGroupFilter)
    {
        IEnumerable<VariableGroup> filteredVariableGroups;
        if (keyFilter is not null)
        {
            try
            {
                var regex = new Regex(keyFilter.ToLower());

                filteredVariableGroups = FilterWithoutSecrets(vgEntity.VariableGroups, variableGroupFilter)
                .Select(vg => vg)
                .Where(vg => vg.Variables.Keys.ToList().FindAll(key => regex.IsMatch(key)).Count > 0);
            }
            catch (RegexParseException ex)
            {
                _logger.LogError(ex, "Couldn't parse and create regex. Value: {value}.", keyFilter);
                filteredVariableGroups = Enumerable.Empty<VariableGroup>();
            }
        }
        else
        {
            filteredVariableGroups = FilterWithoutSecrets(vgEntity.VariableGroups, variableGroupFilter);
        }

        return filteredVariableGroups;
    }

    private async Task<Status> AddVariablesAsync(IEnumerable<VariableGroup> filteredVariableGroups, string key, string value, CancellationToken cancellationToken)
    {
        var updateCounter = 0;

        foreach (var filteredVariableGroup in filteredVariableGroups)
        {
            try
            {
                var success = await AddVariableAsync(key, value, filteredVariableGroup, cancellationToken);

                if (success)
                {
                    updateCounter++;
                }
            }

            catch (ArgumentException ex)
            {
                _logger.LogDebug(
                    ex,
                    "Key has been added previously. Not a breaking error. Variable group: {variableGroupName}, Key: {key}",
                    filteredVariableGroup.Name,
                    key
                    );
                updateCounter++;
            }

            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Something went wrong during variable addition. Variable group: {variableGroupName}, Key: {key}",
                    filteredVariableGroup.Name,
                    key
                    );
            }
        }
        return updateCounter == filteredVariableGroups.Count() ? Status.Success : Status.Unknown;
    }
}
