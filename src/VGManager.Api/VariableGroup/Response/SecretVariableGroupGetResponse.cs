namespace VGManager.Api.VariableGroup.Response;

public class SecretVariableGroupGetResponse: VariableGroupGetBaseResponse
{
    public string KeyVaultName { get; set; } = null!;
}
