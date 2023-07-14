using Microsoft.TeamFoundation.DistributedTask.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.Organization.Client;
using Microsoft.VisualStudio.Services.WebApi;
using System.IO;
using VGService.Repositories.Interface;

namespace VGService.Repositories;

public class VariableGroupConnectionRepository : IVariableGroupConnectionRepository
{
    private VssConnection _connection;
    private string _project;


    public void Setup(string organization, string project, string pat)
    {
        _project = project;

        var uriString = $"https://dev.azure.com/{organization}";
        Uri uri;
        Uri.TryCreate(uriString, UriKind.Absolute, out uri!);

        var credentials = new VssBasicCredential(string.Empty, pat);
        _connection = new VssConnection(uri, credentials);
    }

    public async Task<IEnumerable<VariableGroup>> GetAll()
    {
        var httpClient = _connection.GetClient<TaskAgentHttpClient>();
        var variableGroups = await httpClient.GetVariableGroupsAsync(_project);
        return variableGroups;
    }

    public async Task Update(VariableGroupParameters variableGroupParameters, int variableGroupId)
    {
        variableGroupParameters.VariableGroupProjectReferences = new List<VariableGroupProjectReference>()
        {
            new VariableGroupProjectReference()
            {
                Name = variableGroupParameters.Name,
                ProjectReference = new ProjectReference()
                {
                    Name = _project
                }
            }
        };

        var httpClient = _connection.GetClient<TaskAgentHttpClient>();
        await httpClient!.UpdateVariableGroupAsync(variableGroupId, variableGroupParameters);
    }
}
