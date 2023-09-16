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
        CreateMap<VariableGroupUpdateRequest, VariableGroupUpdateModel>();
        CreateMap<VariableGroupAddRequest, VariableGroupAddModel>();
        CreateMap<VariableGroupRequest, VariableGroupModel>();

        CreateMap<VariableGroupResultBaseModel, VariableGroupResponse>();
        CreateMap<VariableGroupResultModel, VariableGroupResponse>();
        CreateMap<SecretVariableGroupResultModel, VariableGroupResponse>();
        CreateMap<VariableGroupResultsModel, VariableGroupResponses>();
    }
}
