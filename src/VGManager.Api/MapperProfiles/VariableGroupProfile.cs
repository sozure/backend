using AutoMapper;
using VGManager.Api.Endpoints.VariableGroup.Request;
using VGManager.Api.Endpoints.VariableGroup.Response;
using VGManager.Services.Models.VariableGroups.Requests;
using VGManager.Services.Models.VariableGroups.Results;

namespace VGManager.Api.MapperProfiles;

public class VariableGroupProfile : Profile
{
    public VariableGroupProfile()
    {
        CreateMap<VariableUpdateRequest, VariableGroupUpdateModel>();
        CreateMap<VariableAddRequest, VariableGroupAddModel>();
        CreateMap<VariableRequest, VariableGroupModel>();

        CreateMap<VariableResult, VariableResponse>();
    }
}
