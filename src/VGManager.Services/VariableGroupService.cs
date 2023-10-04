using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.DistributedTask.WebApi;
using System.Text.RegularExpressions;
using VGManager.AzureAdapter.Entities;
using VGManager.AzureAdapter.Interfaces;
using VGManager.Services.Interfaces;
using VGManager.Services.Models.VariableGroups.Requests;
using VGManager.Services.Models.VariableGroups.Results;

namespace VGManager.Services;

public class VariableGroupService : IVariableGroupService
{
    private readonly IVariableGroupAdapter _variableGroupConnectionRepository;
    private string _project = null!;
    private readonly ILogger _logger;

    public VariableGroupService(IVariableGroupAdapter variableGroupConnectionRepository, ILogger<VariableGroupService> logger)
    {
        _variableGroupConnectionRepository = variableGroupConnectionRepository;
        _logger = logger;
    }

    public void SetupConnectionRepository(VariableGroupModel variableGroupModel)
    {
        var project = variableGroupModel.Project;
        _variableGroupConnectionRepository.Setup(
            variableGroupModel.Organization,
            project,
            variableGroupModel.PAT
            );
        _project = project;
    }

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

    private IEnumerable<VariableGroup> FilterWithoutSecrets(IEnumerable<VariableGroup> variableGroups, string filter)
    {
        Regex regex;
        try
        {
            regex = new Regex(filter.ToLower());
        }
        catch (RegexParseException ex)
        {
            _logger.LogError(ex, "Couldn't parse and create regex. Value: {value}.", filter);
            return Enumerable.Empty<VariableGroup>();
        }
        return variableGroups.Where(vg => regex.IsMatch(vg.Name.ToLower()) && vg.Type != "AzureKeyVault").ToList();
    }

    private IEnumerable<VariableGroup> Filter(IEnumerable<VariableGroup> variableGroups, string filter)
    {
        Regex regex;
        try
        {
            regex = new Regex(filter.ToLower());
        }
        catch (RegexParseException ex)
        {
            _logger.LogError(ex, "Couldn't parse and create regex. Value: {value}.", filter);
            return Enumerable.Empty<VariableGroup>();
        }
        return variableGroups.Where(vg => regex.IsMatch(vg.Name.ToLower())).ToList();
    }

    private IEnumerable<KeyValuePair<string, VariableValue>> Filter(IDictionary<string, VariableValue> variables, string filter)
    {
        Regex regex;
        try
        {
            regex = new Regex(filter.ToLower());
        }
        catch (RegexParseException ex)
        {
            _logger.LogError(ex, "Couldn't parse and create regex. Value: {value}.", filter);
            return Enumerable.Empty<KeyValuePair<string, VariableValue>>();
        }
        return variables.Where(v => regex.IsMatch(v.Key.ToLower())).ToList();
    }

    private async Task<Status> DeleteVariablesAsync(VariableGroupModel variableGroupModel, IEnumerable<VariableGroup> filteredVariableGroups, CancellationToken cancellationToken)
    {
        var deletionCounter1 = 0;
        var deletionCounter2 = 0;

        foreach (var filteredVariableGroup in filteredVariableGroups)
        {
            var variableGroupName = filteredVariableGroup.Name;

            var deleteIsNeeded = DeleteVariables(
                filteredVariableGroup,
                variableGroupModel.ValueFilter,
                variableGroupModel.KeyFilter,
                variableGroupName
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

    private static VariableGroupParameters GetVariableGroupParameters(VariableGroup filteredVariableGroup, string variableGroupName)
    {
        return new VariableGroupParameters()
        {
            Name = variableGroupName,
            Variables = filteredVariableGroup.Variables,
            Description = filteredVariableGroup.Description,
            Type = filteredVariableGroup.Type,
        };
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

    private bool DeleteVariables(VariableGroup filteredVariableGroup, string? valueCondition, string keyFilter, string variableGroupName)
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
}
