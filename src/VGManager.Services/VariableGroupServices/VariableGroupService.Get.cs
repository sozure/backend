using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.DistributedTask.WebApi;
using System.Text.RegularExpressions;
using VGManager.AzureAdapter.Entities;
using VGManager.Services.Models.VariableGroups.Requests;
using VGManager.Services.Models.VariableGroups.Results;

namespace VGManager.Services.VariableGroupServices;
public partial class VariableGroupService
{
    public async Task<VariableGroupResults> GetVariableGroupsAsync(
        VariableGroupModel variableGroupModel,
        CancellationToken cancellationToken = default
        )
    {
        var vgEntity = await _variableGroupConnectionRepository.GetAllAsync(cancellationToken);
        var status = vgEntity.Status;

        if (status == Status.Success)
        {
            return GetVariableGroupsAsync(variableGroupModel, vgEntity, status);
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
        VariableGroupEntity vgEntity,
        Status status
        )
    {
        var matchedVariableGroups = new List<VariableGroupResult>();
        var filteredVariableGroups = variableGroupModel.ContainsSecrets ?
                        Filter(vgEntity.VariableGroups, variableGroupModel.VariableGroupFilter) :
                        FilterWithoutSecrets(true, variableGroupModel.VariableGroupFilter, vgEntity.VariableGroups);

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

        if (variableGroupModel.KeyIsRegex ?? false)
        {
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
        }
        else
        {
            foreach (var filteredVariableGroup in filteredVariableGroups)
            {
                matchedVariableGroups.AddRange(
                    GetVariables(keyFilter, valueRegex, filteredVariableGroup)
                    );
            }
        }

        return new()
        {
            Status = status,
            VariableGroups = matchedVariableGroups,
        };
    }

    private IEnumerable<VariableGroupResult> GetVariables(
        string keyFilter,
        Regex? valueRegex,
        VariableGroup filteredVariableGroup
        )
    {
        var filteredVariables = Filter(filteredVariableGroup.Variables, keyFilter);
        return CollectVariables(valueRegex, filteredVariableGroup, filteredVariables);
    }

    private IEnumerable<VariableGroupResult> GetVariables(
        Regex keyRegex,
        Regex? valueRegex,
        VariableGroup filteredVariableGroup
        )
    {
        var filteredVariables = Filter(filteredVariableGroup.Variables, keyRegex);
        return CollectVariables(valueRegex, filteredVariableGroup, filteredVariables);
    }

    private IEnumerable<VariableGroupResult> CollectVariables(
        Regex? valueRegex,
        VariableGroup filteredVariableGroup,
        IEnumerable<KeyValuePair<string, VariableValue>> filteredVariables
        )
    {
        var result = new List<VariableGroupResult>();
        foreach (var filteredVariable in filteredVariables)
        {
            var variableValue = filteredVariable.Value.Value ?? string.Empty;
            if (valueRegex is not null)
            {
                if (valueRegex.IsMatch(variableValue.ToLower()))
                {
                    result.AddRange(
                        AddVariableGroupResult(filteredVariableGroup, filteredVariable, variableValue)
                        );
                }
            }
            else
            {
                result.AddRange(
                    AddVariableGroupResult(filteredVariableGroup, filteredVariable, variableValue)
                    );
            }
        }
        return result;
    }

    private IEnumerable<VariableGroupResult> AddVariableGroupResult(
        VariableGroup filteredVariableGroup,
        KeyValuePair<string, VariableValue> filteredVariable,
        string variableValue
        )
    {
        var subResult = new List<VariableGroupResult>();
        if (filteredVariableGroup.Type == "AzureKeyVault")
        {
            var azProviderData = filteredVariableGroup.ProviderData as AzureKeyVaultVariableGroupProviderData;
            subResult.Add(new VariableGroupResult()
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
            subResult.Add(new VariableGroupResult()
            {
                Project = _project ?? string.Empty,
                SecretVariableGroup = false,
                VariableGroupName = filteredVariableGroup.Name,
                VariableGroupKey = filteredVariable.Key,
                VariableGroupValue = variableValue
            });
        }
        return subResult;
    }
}
