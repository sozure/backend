using AutoMapper;
using VGManager.Api.Changes.Request;
using VGManager.Services.Models.Changes;

namespace VGManager.Api.MapperProfiles;

public class ChangesProfile : Profile
{
    public ChangesProfile()
    {
        CreateMap<ChangesRequest, RequestModel>();
    }
}
