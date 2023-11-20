using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.TeamFoundation.DistributedTask.WebApi;
using System.Text.RegularExpressions;
using VGManager.AzureAdapter.Interfaces;
using VGManager.Repositories.Interfaces.VGRepositories;
using VGManager.Services.Interfaces;
using VGManager.Services.Models.VariableGroups.Requests;
using VGManager.Services.Settings;

namespace VGManager.Services.VariableGroupServices;

public partial class VariableGroupService : IVariableGroupService
{
    private readonly IVariableGroupAdapter _variableGroupConnectionRepository;
    private readonly IVGAddColdRepository _additionColdRepository;
    private readonly IVGDeleteColdRepository _deletionColdRepository;
    private readonly IVGUpdateColdRepository _editionColdRepository;
    private readonly OrganizationSettings _organizationSettings;
    private string _project = null!;
    private readonly ILogger _logger;

    public VariableGroupService(
        IVariableGroupAdapter variableGroupConnectionRepository,
        IVGAddColdRepository additionColdRepository,
        IVGDeleteColdRepository deletedColdRepository,
        IVGUpdateColdRepository editionColdRepository,
        IOptions<OrganizationSettings> organizationSettings,
        ILogger<VariableGroupService> logger
        )
    {
        _variableGroupConnectionRepository = variableGroupConnectionRepository;
        _additionColdRepository = additionColdRepository;
        _deletionColdRepository = deletedColdRepository;
        _editionColdRepository = editionColdRepository;
        _organizationSettings = organizationSettings.Value;
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

    private IEnumerable<VariableGroup> FilterWithoutSecrets(bool filterAsRegex, string filter, IEnumerable<VariableGroup> variableGroups)
    {
        if (filterAsRegex)
        {
            Regex regex;
            try
            {
                regex = new Regex(filter.ToLower(), RegexOptions.None, TimeSpan.FromMilliseconds(5));
            }
            catch (RegexParseException ex)
            {
                _logger.LogError(ex, "Couldn't parse and create regex. Value: {value}.", filter);
                return variableGroups.Where(vg => filter.ToLower() == vg.Name.ToLower() && vg.Type != "AzureKeyVault").ToList();
            }
            return variableGroups.Where(vg => regex.IsMatch(vg.Name.ToLower()) && vg.Type != "AzureKeyVault").ToList();
        }
        return variableGroups.Where(vg => filter.ToLower() == vg.Name.ToLower() && vg.Type != "AzureKeyVault").ToList();
    }

    private IEnumerable<VariableGroup> Filter(IEnumerable<VariableGroup> variableGroups, string filter)
    {
        Regex regex;
        try
        {
            regex = new Regex(filter.ToLower(), RegexOptions.None, TimeSpan.FromMilliseconds(5));
        }
        catch (RegexParseException ex)
        {
            _logger.LogError(ex, "Couldn't parse and create regex. Value: {value}.", filter);
            return variableGroups.Where(vg => filter.ToLower() == vg.Name.ToLower()).ToList();
        }
        return variableGroups.Where(vg => regex.IsMatch(vg.Name.ToLower())).ToList();
    }

    private static IEnumerable<KeyValuePair<string, VariableValue>> Filter(IDictionary<string, VariableValue> variables, Regex regex)
    {
        return variables.Where(v => regex.IsMatch(v.Key.ToLower())).ToList();
    }

    private static IEnumerable<KeyValuePair<string, VariableValue>> Filter(IDictionary<string, VariableValue> variables, string filter)
    {
        return variables.Where(v => filter.ToLower() == v.Key.ToLower()).ToList();
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
