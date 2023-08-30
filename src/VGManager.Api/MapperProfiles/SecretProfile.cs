using AutoMapper;
using VGManager.Api.Secrets.Response;
using VGManager.Services.Models.MatchedModels;

namespace VGManager.Api.MapperProfiles;

public class SecretProfile : Profile
{
    public SecretProfile()
    {
        CreateMap<MatchedSecret, SecretGetResponse>();
        CreateMap<MatchedDeletedSecret, DeletedSecretGetResponse>();
    }
}
