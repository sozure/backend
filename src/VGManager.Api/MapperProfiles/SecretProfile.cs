using AutoMapper;
using VGManager.Api.Secret.Request;
using VGManager.Api.Secrets.Response;
using VGManager.Services.Models.Secrets.Requests;
using VGManager.Services.Models.Secrets.Results;

namespace VGManager.Api.MapperProfiles;

public class SecretProfile : Profile
{
    public SecretProfile()
    {
        CreateMap<SecretRequest, SecretModel>();
        CreateMap<SecretCopyRequest, SecretCopyModel>();

        CreateMap<SecretResult, SecretResponse>();
        CreateMap<SecretResults, SecretResponses>();
        CreateMap<DeletedSecretResult, DeletedSecretResponse>();
    }
}
