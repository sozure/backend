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
            return GetVariableGroupsAsync(variableGroupModel, matchedVariableGroups, vgEntity, status);
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

    private VariableGroupResults GetVariableGroupsAsync(
        VariableGroupModel variableGroupModel,
        List<VariableGroupResult> matchedVariableGroups, 
        VariableGroupEntity vgEntity, 
        Status status
        )
    {
        var filteredVariableGroups = variableGroupModel.ContainsSecrets ?
                        Filter(vgEntity.VariableGroups, variableGroupModel.VariableGroupFilter) :
                        FilterWithoutSecrets(vgEntity.VariableGroups, variableGroupModel.VariableGroupFilter);

        var valueFilter = variableGroupModel.ValueFilter;
        var keyFilter = variableGroupModel.KeyFilter;
        Regex? valueRegex = null;

        if (valueFilter is not null)
        {
            try
            {
                valueRegex = new Regex(valueFilter.ToLower());
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

        Regex keyRegex;
        try
        {
            keyRegex = new Regex(keyFilter.ToLower());
        }
        catch (RegexParseException ex)
        {
            _logger.LogError(ex, "Couldn't parse and create regex. Value: {value}.", keyFilter);
            return new()
            {
                Status = status,
                VariableGroups = matchedVariableGroups,
            };
        }

        foreach (var filteredVariableGroup in filteredVariableGroups)
        {
            matchedVariableGroups.AddRange(
                GetVariables(keyRegex, valueRegex, filteredVariableGroup)
                );
        }

        return new()
        {
            Status = status,
            VariableGroups = matchedVariableGroups,
        };
    }

    private List<VariableGroupResult> GetVariables(
        Regex keyRegex,
        Regex? valueRegex,
        VariableGroup filteredVariableGroup
        )
    {
        var result = new List<VariableGroupResult>();
        var filteredVariables = Filter(filteredVariableGroup.Variables, keyRegex);

        foreach (var filteredVariable in filteredVariables)
        {
            var variableValue = filteredVariable.Value.Value ?? string.Empty;
            if (valueRegex is not null)
            {
                if (valueRegex.IsMatch(variableValue.ToLower()))
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

    private void AddVariableGroupResult(
        VariableGroup filteredVariableGroup, 
        List<VariableGroupResult> result, 
        KeyValuePair<string, VariableValue> filteredVariable, 
        string variableValue
        )
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
}
