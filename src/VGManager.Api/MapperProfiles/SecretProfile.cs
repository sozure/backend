using AutoMapper;
using VGManager.Api.Secrets.Response;
using VGManager.Services.Models.Secrets;

namespace VGManager.Api.MapperProfiles;

public class SecretProfile : Profile
{
    public SecretProfile()
    {
        CreateMap<SecretResultModel, SecretGetResponse>();
        CreateMap<DeletedSecretResultModel, DeletedSecretGetResponse>();
    }
}
