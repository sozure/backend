using AutoMapper;
using VGManager.Api.VariableGroups.Request;
using VGManager.Services.Models;

namespace VGManager.Api.MapperProfiles;

public class VariableGroupProfile : Profile
{
    public VariableGroupProfile()
    {
        CreateMap<VariableGroupGetRequest, VariableGroupGetModel>();
        CreateMap<VariableGroupUpdateRequest, VariableGroupUpdateModel>();
        CreateMap<VariableGroupAddRequest, VariableGroupAddModel>();
    }
}
