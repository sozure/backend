using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.DistributedTask.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using VGManager.Repository.Entities;
using VGManager.Repository.Interfaces;

namespace VGManager.Repository;

public class VariableGroupConnectionRepository : IVariableGroupConnectionRepository
{
    private VssConnection _connection = null!;
    private string _project = null!;

    public void Setup(string organization, string project, string pat)
    {
        _project = project;

        var uriString = $"https://dev.azure.com/{organization}";
        Uri uri;
        Uri.TryCreate(uriString, UriKind.Absolute, out uri!);

        var credentials = new VssBasicCredential(string.Empty, pat);
        _connection = new VssConnection(uri, credentials);
    }

    public async Task<VariableGroupEntity> GetAll(CancellationToken cancellationToken = default)
    {
        try
        {
            var httpClient = _connection.GetClient<TaskAgentHttpClient>(cancellationToken: cancellationToken);
            var variableGroups = await httpClient.GetVariableGroupsAsync(_project, cancellationToken: cancellationToken);
            return GetResult(Status.Success, variableGroups);
        }
        catch (VssUnauthorizedException)
        {
            return GetResult(Status.Unauthorized);
        }
        catch (VssServiceResponseException) 
        {
            return GetResult(Status.ResourceNotFound);
        }
        catch (ProjectDoesNotExistWithNameException)
        {
            return GetResult(Status.ProjectDoesNotExist);
        }
        catch(Exception)
        {
            return GetResult(Status.Unknown);
        }
    }

    public async Task<Status> Update(VariableGroupParameters variableGroupParameters, int variableGroupId, CancellationToken cancellationToken = default)
    {
        variableGroupParameters.VariableGroupProjectReferences = new List<VariableGroupProjectReference>()
        {
            new()
            {
                Name = variableGroupParameters.Name,
                ProjectReference = new()
                {
                    Name = _project
                }
            }
        };

        try
        {
            var httpClient = _connection.GetClient<TaskAgentHttpClient>(cancellationToken: cancellationToken);
            await httpClient!.UpdateVariableGroupAsync(variableGroupId, variableGroupParameters, cancellationToken: cancellationToken);
            return Status.Success;
        }
        catch (VssUnauthorizedException)
        {
            return Status.Unauthorized;
        }
        catch (VssServiceResponseException)
        {
            return Status.ResourceNotFound;
        }
        catch (ProjectDoesNotExistWithNameException)
        {
            return Status.ProjectDoesNotExist;
        }
        catch (Exception)
        {
            return Status.Unknown;
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
