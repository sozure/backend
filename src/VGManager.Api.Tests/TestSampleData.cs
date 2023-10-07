
using Azure.Security.KeyVault.Secrets;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.DistributedTask.WebApi;
using VGManager.Api.Secret.Request;
using VGManager.Api.Secrets.Response;
using VGManager.Api.VariableGroups.Request;
using VGManager.Api.VariableGroups.Response;
using VGManager.AzureAdapter.Entities;

namespace VGManager.Api.Tests;
public static class TestSampleData
{
    public static VariableGroupAddRequest GetVariableAddRequest(
        string organization,
        string pat,
        string project,
        string valueFilter,
        string newKey,
        string newValue
        )
    {
        return new VariableGroupAddRequest
        {
            Organization = organization,
            PAT = pat,
            Project = project,
            VariableGroupFilter = "neptun",
            KeyFilter = null!,
            ValueFilter = valueFilter,
            Key = newKey,
            Value = newValue
        };
    }

    public static VariableGroupUpdateRequest GetVariableUpdateRequest(
        string variableGroupFilter,
        string organization,
        string pat,
        string project,
        string valueFilter,
        string newValue
        )
    {
        return new VariableGroupUpdateRequest
        {
            Organization = organization,
            PAT = pat,
            Project = project,
            VariableGroupFilter = variableGroupFilter,
            KeyFilter = "Key123",
            ValueFilter = valueFilter,
            ContainsSecrets = false,
            NewValue = newValue
        };
    }

    public static VariableGroupRequest GetVariableRequest(string organization, string pat, string project, string keyFilter, string valueFilter)
    {
        return new VariableGroupRequest
        {
            Organization = organization,
            PAT = pat,
            Project = project,
            VariableGroupFilter = "neptun",
            KeyFilter = keyFilter,
            ValueFilter = valueFilter,
            KeyIsRegex = true,
            ContainsSecrets = false
        };
    }

    public static VariableGroupRequest GetVariableRequest(string organization, string pat, string project)
    {
        return new VariableGroupRequest
        {
            Organization = organization,
            PAT = pat,
            Project = project,
            VariableGroupFilter = "neptun",
            KeyFilter = "key",
            ContainsSecrets = false,
            KeyIsRegex = true,
        };
    }

    public static VariableGroupEntity GetVariableGroupEntity()
    {
        return new VariableGroupEntity
        {
            Status = Status.Success,
            VariableGroups = new List<VariableGroup>
            {
                new()
                {
                    Name = "NeptunAdapter",
                    Variables = new Dictionary<string, VariableValue>
                    {
                        ["Key123"] = new()
                        {
                            Value = "Value123"
                        },
                        ["Key456"] = new()
                        {
                            Value = "Value456"
                        }
                    }
                },
                new()
                {
                    Name = "NeptunApi",
                    Variables = new Dictionary<string, VariableValue>
                    {
                        ["Key789"] = new()
                        {
                            Value = "Value789"
                        },
                        ["Kec"] = new()
                        {
                            Value = "Valuc"
                        }
                    }
                },
            }
        };
    }

    public static VariableGroupEntity GetVariableGroupEntityAfterDelete()
    {
        return new VariableGroupEntity
        {
            Status = Status.Success,
            VariableGroups = new List<VariableGroup>()
        };
    }

    public static VariableGroupEntity GetVariableGroupEntity(string value)
    {
        return new VariableGroupEntity
        {
            Status = Status.Success,
            VariableGroups = new List<VariableGroup>
            {
                new()
                {
                    Name = "NeptunAdapter",
                    Variables = new Dictionary<string, VariableValue>
                    {
                        ["Key123"] = new()
                        {
                            Value = value
                        },
                        ["Key456"] = new()
                        {
                            Value = value
                        }
                    }
                },
                new()
                {
                    Name = "NeptunApi",
                    Variables = new Dictionary<string, VariableValue>
                    {
                        ["Key789"] = new()
                        {
                            Value = value
                        }
                    }
                },
            }
        };
    }

    public static VariableGroupEntity GetVariableGroupEntity(string key, string value)
    {
        return new VariableGroupEntity
        {
            Status = Status.Success,
            VariableGroups = new List<VariableGroup>
            {
                new()
                {
                    Name = "NeptunAdapter",
                    Variables = new Dictionary<string, VariableValue>
                    {
                        [key] = new()
                        {
                            Value = value
                        }
                    }
                },
                new()
                {
                    Name = "NeptunApi",
                    Variables = new Dictionary<string, VariableValue>
                    {
                        [key] = new()
                        {
                            Value = value
                        }
                    }
                },
            }
        };
    }

    public static VariableGroupResponses GetVariableGroupGetResponses(string projectName, string key, string value)
    {
        var list = new List<VariableGroupResponse>()
        {
                new()
                {
                    Project = projectName,
                    VariableGroupName = "NeptunAdapter",
                    VariableGroupKey = key,
                    VariableGroupValue = value
                },
                new()
                {
                    Project = projectName,
                    VariableGroupName = "NeptunApi",
                    VariableGroupKey = key,
                    VariableGroupValue = value
                }
        };

        var result = new List<VariableGroupResponse>();

        foreach (var item in list)
        {
            result.Add(item);
        }

        return new VariableGroupResponses
        {
            Status = Status.Success,
            VariableGroups = result
        };
    }

    public static VariableGroupResponses GetVariableGroupGetResponses(string projectName, string value)
    {
        var list = new List<VariableGroupResponse>()
        {
                new()
                {
                    Project = projectName,
                    VariableGroupName = "NeptunAdapter",
                    VariableGroupKey = "Key123",
                    VariableGroupValue = value
                }
        };

        var result = new List<VariableGroupResponse>();

        foreach (var item in list)
        {
            result.Add(item);
        }

        return new VariableGroupResponses
        {
            Status = Status.Success,
            VariableGroups = result
        };
    }

    public static VariableGroupResponses GetVariableGroupGetResponses(string projectName)
    {
        var list = new List<VariableGroupResponse>()
        {
            new()
            {
                Project = projectName,
                VariableGroupName = "NeptunAdapter",
                VariableGroupKey = "Key123",
                VariableGroupValue = "Value123"
            },
            new()
            {
                Project = projectName,
                VariableGroupName = "NeptunAdapter",
                VariableGroupKey = "Key456",
                VariableGroupValue = "Value456"
            },
            new()
            {
                Project = projectName,
                VariableGroupName = "NeptunApi",
                VariableGroupKey = "Key789",
                VariableGroupValue = "Value789"
            }
        };

        var result = new List<VariableGroupResponse>();

        foreach (var item in list)
        {
            result.Add(item);
        }

        return new VariableGroupResponses
        {
            Status = Status.Success,
            VariableGroups = result
        };
    }

    public static VariableGroupResponses GetVariableGroupGetResponsesAfterDelete()
    {
        return new VariableGroupResponses
        {
            Status = Status.Success,
            VariableGroups = new List<VariableGroupResponse>()
        };
    }

    public static SecretRequest GetRequest(string keyVaultName, string secretFilter, string tenantId, string clientId, string clientSecret)
    {
        return new SecretRequest
        {
            KeyVaultName = keyVaultName,
            SecretFilter = secretFilter,
            TenantId = tenantId,
            ClientId = clientId,
            ClientSecret = clientSecret
        };
    }

    public static SecretCopyRequest GetRequest(string fromKeyVault, string toKeyVault, string tenantId, string clientId, string clientSecret, bool overrideSecret)
    {
        return new SecretCopyRequest
        {
            TenantId = tenantId,
            ClientId = clientId,
            ClientSecret = clientSecret,
            FromKeyVault = fromKeyVault,
            ToKeyVault = toKeyVault,
            overrideSecret = overrideSecret
        };
    }

    public static SecretResponses GetSecretsGetResponse()
    {
        return new SecretResponses
        {
            Status = Status.Success,
            Secrets = new List<SecretResponse>()
            {
                new()
                {
                    KeyVault = "KeyVaultName1",
                    SecretName = "SecretFilter123",
                    SecretValue = "3Kpu6gF214vAqHlzaX5G"
                },
                new()
                {
                    KeyVault = "KeyVaultName1",
                    SecretName = "SecretFilter456",
                    SecretValue = "KCRQJ08PdFHU9Ly2pUI2"
                },
                new()
                {
                    KeyVault = "KeyVaultName1",
                    SecretName = "SecretFilter789",
                    SecretValue = "ggl1oBLSiYNBliNQhsGW"
                }
            }
        };
    }

    public static SecretsEntity GetSecretsEntity()
    {
        return new SecretsEntity
        {
            Status = Status.Success,
            Secrets = new List<SecretEntity>()
            {
                new()
                {
                    Status = Status.Success,
                    Secret = new("SecretFilter123", "3Kpu6gF214vAqHlzaX5G")
                },
                new()
                {
                    Status = Status.Success,
                    Secret = new("SecretFilter456", "KCRQJ08PdFHU9Ly2pUI2")
                },
                new()
                {
                    Status = Status.Success,
                    Secret = new("SecretFilter789", "ggl1oBLSiYNBliNQhsGW")
                }
            }
        };
    }

    public static DeletedSecretsEntity GetEmptyDeletedSecretsEntity()
    {
        return new DeletedSecretsEntity
        {
            Status = Status.Success,
            DeletedSecrets = Enumerable.Empty<DeletedSecret>()
        };
    }

    public static SecretsEntity GetEmptySecretsEntity()
    {
        return new SecretsEntity
        {
            Status = Status.Success,
            Secrets = Enumerable.Empty<SecretEntity>()
        };
    }

    public static SecretResponses GetEmptySecretsGetResponse()
    {
        return new SecretResponses
        {
            Status = Status.Success,
            Secrets = Enumerable.Empty<SecretResponse>()

        };
    }

    public static DeletedSecretResponses GetEmptySecretsGetResponse1()
    {
        return new DeletedSecretResponses
        {
            Status = Status.Success,
            DeletedSecrets = Enumerable.Empty<DeletedSecretResponse>()
        };
    }

    public static ProjectEntity GetProjectEntity(string firstProjectName, string secondProjectName)
    {
        return new ProjectEntity
        {
            Status = Status.Success,
            Projects = new List<TeamProjectReference>
            {
                new()
                {
                    Name = firstProjectName,
                },
                new()
                {
                    Name = secondProjectName,
                }
            }
        };
    }
}
