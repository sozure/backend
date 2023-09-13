using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.DistributedTask.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using VGManager.AzureAdapter.Entities;
using VGManager.AzureAdapter.Interfaces;

namespace VGManager.AzureAdapter;

public class VariableGroupAdapter : IVariableGroupAdapter
{
    private VssConnection _connection = null!;
    private string _project = null!;
    private readonly ILogger _logger;

    public VariableGroupAdapter(ILogger<VariableGroupAdapter> logger)
    {
        _logger = logger;
    }

    public void Setup(string organization, string project, string pat)
    {
        _project = project;

        var uriString = $"https://dev.azure.com/{organization}";
        Uri uri;
        Uri.TryCreate(uriString, UriKind.Absolute, out uri!);

        var credentials = new VssBasicCredential(string.Empty, pat);
        _connection = new VssConnection(uri, credentials);
    }

    public async Task<VariableGroupEntity> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Request variable groups from {project} Azure project.", _project);
            var httpClient = await _connection.GetClientAsync<TaskAgentHttpClient>(cancellationToken: cancellationToken);
            var variableGroups = await httpClient.GetVariableGroupsAsync(_project, cancellationToken: cancellationToken);
            return GetResult(Status.Success, variableGroups);
        }
        catch (VssUnauthorizedException ex)
        {
            var status = Status.Unauthorized;
            _logger.LogError(ex, "Couldn't get variable groups. Status: {status}.", status);
            return GetResult(status);
        }
        catch (VssServiceResponseException ex)
        {
            var status = Status.ResourceNotFound;
            _logger.LogError(ex, "Couldn't get variable groups. Status: {status}.", status);
            return GetResult(status);
        }
        catch (ProjectDoesNotExistWithNameException ex)
        {
            var status = Status.ProjectDoesNotExist;
            _logger.LogError(ex, "Couldn't get variable groups. Status: {status}.", status);
            return GetResult(status);
        }
        catch (Exception ex)
        {
            var status = Status.Unknown;
            _logger.LogError(ex, "Couldn't get variable groups. Status: {status}.", status);
            return GetResult(status);
        }
    }

    public async Task<Status> UpdateAsync(VariableGroupParameters variableGroupParameters, int variableGroupId, CancellationToken cancellationToken = default)
    {
        var variableGroupName = variableGroupParameters.Name;
        variableGroupParameters.VariableGroupProjectReferences = new List<VariableGroupProjectReference>()
        {
            new()
            {
                Name = variableGroupName,
                ProjectReference = new()
                {
                    Name = _project
                }
            }
        };

        try
        {
            _logger.LogInformation("Update variable group with name: {variableGroupName} in {project} Azure project.", variableGroupName, _project);
            var httpClient = await _connection.GetClientAsync<TaskAgentHttpClient>(cancellationToken: cancellationToken);
            await httpClient!.UpdateVariableGroupAsync(variableGroupId, variableGroupParameters, cancellationToken: cancellationToken);
            return Status.Success;
        }
        catch (VssUnauthorizedException ex)
        {
            var status = Status.Unauthorized;
            _logger.LogError(ex, "Couldn't update variable groups. Status: {status}.", status);
            return status;
        }
        catch (VssServiceResponseException ex)
        {
            var status = Status.ResourceNotFound;
            _logger.LogError(ex, "Couldn't update variable groups. Status: {status}.", status);
            return status;
        }
        catch (ProjectDoesNotExistWithNameException ex)
        {
            var status = Status.ProjectDoesNotExist;
            _logger.LogError(ex, "Couldn't update variable groups. Status: {status}.", status);
            return status;
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, $"An item with the same key has already been added to {variableGroupName}.");
            return Status.Unknown;
        }
        catch (TeamFoundationServerInvalidRequestException ex)
        {
            _logger.LogError(ex, $"Wasn't added to {variableGroupName} because of TeamFoundationServerInvalidRequestException.");
            return Status.Unknown;
        }
        catch (Exception ex)
        {
            var status = Status.Unknown;
            _logger.LogError(ex, "Couldn't update variable groups. Status: {status}.", status);
            return status;
        }
    }

    private static VariableGroupEntity GetResult(Status status, IEnumerable<VariableGroup> variableGroups)
    {
        return new()
        {
            Status = status,
            VariableGroups = variableGroups
        };
    }

    private static VariableGroupEntity GetResult(Status status)
    {
        return new()
        {
            Status = status,
            VariableGroups = Enumerable.Empty<VariableGroup>()
        };
    }
}
