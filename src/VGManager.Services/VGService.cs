using Microsoft.TeamFoundation;
using Microsoft.TeamFoundation.DistributedTask.WebApi;
using System.Text.RegularExpressions;
using VGManager.Services.Interfaces;
using VGManager.Services.Model;
using VGManager.Services.Repositories.Interface;

namespace VGManager.Services;

public class VariableGroupService : IVariableGroupService
{
    public async Task<IEnumerable<MatchedVariableGroup>> GetVariableGroupsAsync(
        IVariableGroupConnectionRepository connectionService, 
        string variableGroupFilter,
        string keyFilter, 
        string valueFilter
        )
    {
        var matchedVariableGroups = new List<MatchedVariableGroup>();
        var variableGroups = await connectionService.GetAll();
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
        IVariableGroupConnectionRepository connectionService,
        string variableGroupFilter, string keyFilter,
        string newValue, string valueCondition
        )
    {
        Console.WriteLine("Variable group name, Key, Old value, New value");
        var variableGroups = await connectionService.GetAll();
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
                await connectionService.Update(variableGroupParameters, filteredVariableGroup.Id);
                Console.WriteLine($"{variableGroupName} updated.");
            }
        }
    }

    public async Task AddVariableAsync(IVariableGroupConnectionRepository connectionService,
        string variableGroupFilter, string keyFilter, string key,
        string newValue)
    {
        var variableGroups = await connectionService.GetAll();
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
                await connectionService.Update(variableGroupParameters, filteredVariableGroup.Id);
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

    public async Task DeleteVariableAsync(IVariableGroupConnectionRepository connectionService,
        string variableGroupFilter, string keyFilter, string valueCondition)
    {
        Console.WriteLine("Variable group name, Deleted Key, Deleted Value");
        var variableGroups = await connectionService.GetAll();
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
                await connectionService.Update(variableGroupParameters, filteredVariableGroup.Id);
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
