using AutoMapper;
using VGManager.Api.VariableGroups.Request;
using VGManager.Api.VariableGroups.Response;
using VGManager.Services.Models.VariableGroups;
using VGManager.Services.Models.VariableGroups.Results;

namespace VGManager.Api.MapperProfiles;

public class VariableGroupProfile : Profile
{
    public VariableGroupProfile()
    {
        CreateMap<VariableGroupGetRequest, VariableGroupGetModel>();
        CreateMap<VariableGroupUpdateRequest, VariableGroupUpdateModel>();
        CreateMap<VariableGroupAddRequest, VariableGroupAddModel>();
        CreateMap<VariableGroupDeleteRequest, VariableGroupDeleteModel>();
        CreateMap<VariableGroupResultModel, VariableGroupGetResponse>();
        CreateMap<VariableGroupResultsModel, VariableGroupGetResponses>();
    }
}
