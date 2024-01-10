using AutoMapper;
using VGManager.Api.VariableGroups.Request;
using VGManager.Api.VariableGroups.Response;
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
