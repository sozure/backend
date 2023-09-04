using VGManager.Repository.Entities;

namespace VGManager.Api.VariableGroups.Response;

public class VariableGroupGetResponses
{
    public Status Status { get; set; }
    public IEnumerable<VariableGroupGetResponse> VariableGroups { get; set; } = null!;
}
