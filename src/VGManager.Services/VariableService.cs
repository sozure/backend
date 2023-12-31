using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.TeamFoundation.DistributedTask.WebApi;
using System.Text.RegularExpressions;
using VGManager.AzureAdapter.Interfaces;
using VGManager.Repositories.Interfaces.VGRepositories;
using VGManager.Services.Interfaces;
using VGManager.Services.Models.VariableGroups.Requests;
using VGManager.Services.Settings;

namespace VGManager.Services;

public partial class VariableService : IVariableService
{
    private readonly IVariableGroupAdapter _variableGroupConnectionRepository;
    private readonly IVGAddColdRepository _additionColdRepository;
    private readonly IVGDeleteColdRepository _deletionColdRepository;
    private readonly IVGUpdateColdRepository _editionColdRepository;
    private readonly IVariableFilterService _variableFilterService;
    private readonly OrganizationSettings _organizationSettings;
    private string _project = null!;
    private readonly ILogger _logger;

    public VariableService(
        IVariableGroupAdapter variableGroupConnectionRepository,
        IVGAddColdRepository additionColdRepository,
        IVGDeleteColdRepository deletedColdRepository,
        IVGUpdateColdRepository editionColdRepository,
        IVariableFilterService variableFilterService,
        IOptions<OrganizationSettings> organizationSettings,
        ILogger<VariableService> logger
        )
    {
        _variableGroupConnectionRepository = variableGroupConnectionRepository;
        _additionColdRepository = additionColdRepository;
        _deletionColdRepository = deletedColdRepository;
        _editionColdRepository = editionColdRepository;
        _variableFilterService = variableFilterService;
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
