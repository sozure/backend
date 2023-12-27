using AutoMapper;
using VGManager.Api.Changes.Request;
using VGManager.Services.Models.Changes.Requests;

namespace VGManager.Api.MapperProfiles;

public class ChangesProfile : Profile
{
    public ChangesProfile()
    {
        CreateMap<VGChangesRequest, VGRequestModel>();
        CreateMap<SecretChangesRequest, SecretRequestModel>();
        CreateMap<KVChangesRequest, KVRequestModel>();
    }
}
