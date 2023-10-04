using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.DistributedTask.WebApi;
using System.Text.RegularExpressions;
using VGManager.AzureAdapter.Interfaces;
using VGManager.Services.Interfaces;
using VGManager.Services.Models.VariableGroups.Requests;

namespace VGManager.Services;

public partial class VariableGroupService : IVariableGroupService
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
