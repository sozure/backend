using Microsoft.TeamFoundation;
using Microsoft.TeamFoundation.DistributedTask.WebApi;
using System.Text.RegularExpressions;
using VGManager.Repository.Entities;
using VGManager.Repository.Interfaces;
using VGManager.Services.Interfaces;
using VGManager.Services.Models.VariableGroups;
using VGManager.Services.Models.VariableGroups.Results;

namespace VGManager.Services;

public class VariableGroupService : IVariableGroupService
{
    private readonly IVariableGroupConnectionRepository _variableGroupConnectionRepository;

    public VariableGroupService(IVariableGroupConnectionRepository variableGroupConnectionRepository)
    {
        _variableGroupConnectionRepository = variableGroupConnectionRepository;
    }

    public void SetupConnectionRepository(VariableGroupModel variableGroupModel)
    {
        _variableGroupConnectionRepository.Setup(variableGroupModel.Organization, variableGroupModel.Project, variableGroupModel.PAT);
    }

    public async Task<VariableGroupResultsModel> GetVariableGroupsAsync(
        VariableGroupModel variableGroupModel,
        CancellationToken cancellationToken = default
        )
    {
        var matchedVariableGroups = new List<VariableGroupResultModel>();
        var vgEntity = await _variableGroupConnectionRepository.GetAll(cancellationToken);
        var status = vgEntity.Status;

        if(status == Status.Success)
        {
            var filteredVariableGroups = Filter(vgEntity.VariableGroups, variableGroupModel.VariableGroupFilter);
            Regex regex = null!;

            var valueFilter = variableGroupModel.ValueFilter;

            if (valueFilter is not null)
            {
                regex = new Regex(valueFilter);
            }

            foreach (var filteredVariableGroup in filteredVariableGroups)
            {
                GetVariables(variableGroupModel.KeyFilter, valueFilter, matchedVariableGroups, regex, filteredVariableGroup);
            }
            return new()
            {
                Status = status,
                VariableGroups = matchedVariableGroups,
            };
        } else
        {
            return new()
            {
                Status = status,
                VariableGroups = Enumerable.Empty<VariableGroupResultModel>(),
            };
        }
    }

    public async Task<Status> UpdateVariableGroupsAsync(
        VariableGroupUpdateModel variableGroupUpdateModel, 
        CancellationToken cancellationToken = default
        )
    {
        Console.WriteLine("Variable group name, Key, Old value, New value");
        var vgEntity = await _variableGroupConnectionRepository.GetAll(cancellationToken);
        var status = vgEntity.Status;

        if(status == Status.Success)
        {
            var filteredVariableGroups = Filter(vgEntity.VariableGroups, variableGroupUpdateModel.VariableGroupFilter);

            foreach (var filteredVariableGroup in filteredVariableGroups)
            {
                var variableGroupName = filteredVariableGroup.Name;
                var updateIsNeeded = UpdateVariables(variableGroupUpdateModel, filteredVariableGroup, variableGroupName);

                if (updateIsNeeded)
                {
                    var variableGroupParameters = GetVariableGroupParameters(filteredVariableGroup, variableGroupName);
                    await _variableGroupConnectionRepository.Update(variableGroupParameters, filteredVariableGroup.Id, cancellationToken);
                    Console.WriteLine($"{variableGroupName} updated.");
                }
            }
        }
        return status;
    }

    public async Task<Status> AddVariableAsync(VariableGroupAddModel variableGroupAddModel, CancellationToken cancellationToken = default)
    {
        var vgEntity = await _variableGroupConnectionRepository.GetAll(cancellationToken);
        var status = vgEntity.Status;

        if (status == Status.Success)
        {
            IEnumerable<VariableGroup> filteredVariableGroups = null!;
            var keyFilter = variableGroupAddModel.KeyFilter;
            var variableGroupFilter = variableGroupAddModel.VariableGroupFilter;
            var key = variableGroupAddModel.Key;
            var value = variableGroupAddModel.Value;

            if (keyFilter is not null)
            {
                var regex = new Regex(keyFilter);

                filteredVariableGroups = Filter(vgEntity.VariableGroups, variableGroupFilter)
                    .Select(vg => vg)
                    .Where(vg => vg.Variables.Keys.ToList().FindAll(key => regex.IsMatch(key)).Count > 0);
            }
            else
            {
                filteredVariableGroups = Filter(vgEntity.VariableGroups, variableGroupFilter);
            }

            foreach (var filteredVariableGroup in filteredVariableGroups)
            {
                var variableGroupName = filteredVariableGroup.Name;
                try
                {
                    filteredVariableGroup.Variables.Add(key, value);
                    var variableGroupParameters = GetVariableGroupParameters(filteredVariableGroup, variableGroupName);
                    await _variableGroupConnectionRepository.Update(variableGroupParameters, filteredVariableGroup.Id, cancellationToken);
                    Console.WriteLine($"{variableGroupName}, {key}, {value}");
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
        return status;
    }

    public async Task<Status> DeleteVariableAsync(VariableGroupDeleteModel variableGroupDeleteModel, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Variable group name, Deleted Key, Deleted Value");
        var vgEntity = await _variableGroupConnectionRepository.GetAll(cancellationToken);
        var status = vgEntity.Status;

        if(status == Status.Success)
        {
            var filteredVariableGroups = Filter(vgEntity.VariableGroups, variableGroupDeleteModel.VariableGroupFilter);

            foreach (var filteredVariableGroup in filteredVariableGroups)
            {
                var variableGroupName = filteredVariableGroup.Name;

                var deleteIsNeeded = DeleteVariables(
                    filteredVariableGroup,
                    variableGroupDeleteModel.ValueFilter,
                    variableGroupDeleteModel.KeyFilter,
                    variableGroupName
                    );

                if (deleteIsNeeded)
                {
                    var variableGroupParameters = GetVariableGroupParameters(filteredVariableGroup, variableGroupName);
                    await _variableGroupConnectionRepository.Update(variableGroupParameters, filteredVariableGroup.Id, cancellationToken);
                }
            }
        }

        return status;
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

    private static void GetVariables(
        string keyFilter,
        string? valueFilter,
        List<VariableGroupResultModel> matchedVariableGroups,
        Regex regex,
        VariableGroup filteredVariableGroup
        )
    {
        var filteredVariables = Filter(filteredVariableGroup.Variables, keyFilter);

        foreach (var filteredVariable in filteredVariables)
        {
            var variableValue = filteredVariable.Value.Value ?? string.Empty;
            if (valueFilter is not null)
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

    private static bool UpdateVariables(
        VariableGroupUpdateModel variableGroupUpdateModel,
        VariableGroup filteredVariableGroup,
        string variableGroupName
        )
    {
        var filteredVariables = Filter(filteredVariableGroup.Variables, variableGroupUpdateModel.KeyFilter);
        var updateIsNeeded = false;

        foreach (var filteredVariable in filteredVariables)
        {
            var variableValue = filteredVariable.Value.Value;
            var variableKey = filteredVariable.Key;
            var newValue = variableGroupUpdateModel.NewValue;
            var valueFilter = variableGroupUpdateModel.ValueFilter;

            if (valueFilter is not null)
            {
                if (valueFilter.Equals(variableValue))
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

        return updateIsNeeded;
    }

    private static bool DeleteVariables(VariableGroup filteredVariableGroup, string? valueCondition, string keyFilter, string variableGroupName)
    {
        var deleteIsNeeded = false;
        var filteredVariables = Filter(filteredVariableGroup.Variables, keyFilter);
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

        return deleteIsNeeded;
    }
}
