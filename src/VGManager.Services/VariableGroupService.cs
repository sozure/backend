using Microsoft.TeamFoundation;
using Microsoft.TeamFoundation.DistributedTask.WebApi;
using System.Text.RegularExpressions;
using VGManager.Repository.Interfaces;
using VGManager.Services.Interfaces;
using VGManager.Services.Model;

namespace VGManager.Services;

public class VariableGroupService : IVariableGroupService
{
    private readonly IVariableGroupConnectionRepository _variableGroupConnectionRepository;

    public VariableGroupService(IVariableGroupConnectionRepository variableGroupConnectionRepository)
    {
        _variableGroupConnectionRepository = variableGroupConnectionRepository;
    }

    public void SetupConnectionRepository(string organization, string project, string pat)
    {
        _variableGroupConnectionRepository.Setup(organization, project, pat);
    }

    public async Task<IEnumerable<MatchedVariableGroup>> GetVariableGroupsAsync(
        string variableGroupFilter,
        string keyFilter,
        string valueFilter,
        CancellationToken cancellationToken = default
        )
    {
        var matchedVariableGroups = new List<MatchedVariableGroup>();
        var variableGroups = await _variableGroupConnectionRepository.GetAll(cancellationToken);
        var filteredVariableGroups = Filter(variableGroups, variableGroupFilter);
        Regex regex = null!;

        if (valueFilter != null)
        {
            regex = new Regex(valueFilter);
        }

        foreach (var filteredVariableGroup in filteredVariableGroups)
        {
            var filteredVariables = Filter(filteredVariableGroup.Variables, keyFilter);

            foreach (var filteredVariable in filteredVariables)
            {
                var variableValue = filteredVariable.Value.Value ?? string.Empty;
                if (valueFilter != null)
                {
                    if (regex.IsMatch(variableValue))
                    {
                        matchedVariableGroups.Add(new()
                        {
                            VariableGroupName = filteredVariableGroup.Name,
                            VariableGroupKey = filteredVariable.Key,
                            VariableGroupValue = variableValue
                        });
                    }
                }
                else
                {
                    matchedVariableGroups.Add(new()
                    {
                        VariableGroupName = filteredVariableGroup.Name,
                        VariableGroupKey = filteredVariable.Key,
                        VariableGroupValue = variableValue
                    });
                }
            }
        }
        return matchedVariableGroups;
    }

    public async Task UpdateVariableGroupsAsync(
        string variableGroupFilter, 
        string keyFilter, 
        string newValue, 
        string valueCondition, 
        CancellationToken cancellationToken = default
        )
    {
        Console.WriteLine("Variable group name, Key, Old value, New value");
        var variableGroups = await _variableGroupConnectionRepository.GetAll(cancellationToken);
        var filteredVariableGroups = Filter(variableGroups, variableGroupFilter);

        foreach (var filteredVariableGroup in filteredVariableGroups)
        {
            var filteredVariables = Filter(filteredVariableGroup.Variables, keyFilter);
            var variableGroupName = filteredVariableGroup.Name;
            var updateIsNeeded = false;

            foreach (var filteredVariable in filteredVariables)
            {
                var variableValue = filteredVariable.Value.Value;
                var variableKey = filteredVariable.Key;

                if (valueCondition is not null)
                {
                    if (valueCondition.Equals(variableValue))
                    {
                        Console.WriteLine($"{variableGroupName}, {variableKey}, {variableValue}, {newValue}");
                        filteredVariable.Value.Value = newValue;
                        updateIsNeeded = true;
                    }
                }
                else
                {
                    Console.WriteLine($"{variableGroupName}, {variableKey}, {variableValue}, {newValue}");
                    filteredVariable.Value.Value = newValue;
                    updateIsNeeded = true;
                }
            }

            if (updateIsNeeded)
            {
                var variableGroupParameters = GetVariableGroupParameters(filteredVariableGroup, variableGroupName);
                await _variableGroupConnectionRepository.Update(variableGroupParameters, filteredVariableGroup.Id, cancellationToken);
                Console.WriteLine($"{variableGroupName} updated.");
            }
        }
    }

    public async Task AddVariableAsync(
        string variableGroupFilter, 
        string keyFilter, 
        string key, 
        string newValue, 
        CancellationToken cancellationToken = default
        )
    {
        var variableGroups = await _variableGroupConnectionRepository.GetAll(cancellationToken);
        IEnumerable<VariableGroup> filteredVariableGroups = null!;

        if (keyFilter is not null)
        {
            var regex = new Regex(keyFilter);

            filteredVariableGroups = Filter(variableGroups, variableGroupFilter)
                .Select(vg => vg)
                .Where(vg => vg.Variables.Keys.ToList().FindAll(key => regex.IsMatch(key)).Count > 0);
        }
        else
        {
            filteredVariableGroups = Filter(variableGroups, variableGroupFilter);
        }

        foreach (var filteredVariableGroup in filteredVariableGroups)
        {
            var variableGroupName = filteredVariableGroup.Name;
            try
            {
                filteredVariableGroup.Variables.Add(key, newValue);
                var variableGroupParameters = GetVariableGroupParameters(filteredVariableGroup, variableGroupName);
                await _variableGroupConnectionRepository.Update(variableGroupParameters, filteredVariableGroup.Id, cancellationToken);
                Console.WriteLine($"{variableGroupName}, {key}, {newValue}");
            }
            catch (ArgumentException)
            {
                Console.WriteLine($"An item with the same '{key}' key has already been added to {variableGroupName}");
            }

            catch (TeamFoundationServerInvalidRequestException)
            {
                Console.WriteLine($"Wasn't added to {variableGroupName} because of TeamFoundationServerInvalidRequestException");
            }
        }
    }

    public async Task DeleteVariableAsync(
        string variableGroupFilter, 
        string keyFilter, 
        string valueCondition, 
        CancellationToken cancellationToken = default
        )
    {
        Console.WriteLine("Variable group name, Deleted Key, Deleted Value");
        var variableGroups = await _variableGroupConnectionRepository.GetAll(cancellationToken);
        var filteredVariableGroups = Filter(variableGroups, variableGroupFilter);


        foreach (var filteredVariableGroup in filteredVariableGroups)
        {
            var filteredVariables = Filter(filteredVariableGroup.Variables, keyFilter);
            var deleteIsNeeded = false;
            var variableGroupName = filteredVariableGroup.Name;

            foreach (var filteredVariable in filteredVariables)
            {
                var variableValue = filteredVariable.Value.Value;
                var variableKey = filteredVariable.Key;

                if (valueCondition is not null)
                {
                    if (valueCondition.Equals(variableValue))
                    {
                        Console.WriteLine($"{variableGroupName}, {variableKey}, {variableValue}");
                        filteredVariableGroup.Variables.Remove(filteredVariable.Key);
                        deleteIsNeeded = true;
                    }
                }
                else
                {
                    Console.WriteLine($"{variableGroupName}, {variableKey}, {variableValue}");
                    filteredVariableGroup.Variables.Remove(filteredVariable.Key);
                    deleteIsNeeded = true;
                }
            }
            if (deleteIsNeeded)
            {
                var variableGroupParameters = GetVariableGroupParameters(filteredVariableGroup, variableGroupName);
                await _variableGroupConnectionRepository.Update(variableGroupParameters, filteredVariableGroup.Id, cancellationToken);
            }
        }
    }

    protected static IEnumerable<VariableGroup> Filter(IEnumerable<VariableGroup> variableGroups, string filter)
    {
        var regex = new Regex(filter);
        return variableGroups.Where(vg => regex.IsMatch(vg.Name)).ToList();
    }

    protected static IEnumerable<KeyValuePair<string, VariableValue>> Filter(IDictionary<string, VariableValue> variables, string filter)
    {
        var regex = new Regex(filter);
        return variables.Where(v => regex.IsMatch(v.Key)).ToList();
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
}
