using AutoMapper;
using VGManager.Entities;
using VGManager.Services.Models.Changes;

namespace VGManager.Services.MapperProfiles;

public class ChangesProfile : Profile
{
    public ChangesProfile()
    {
        CreateMap<VGEntity, OperationModel>()
            .ForMember(src => src.Type, o => o.MapFrom(x => ChangeType.None.ToString()));
        CreateMap<VGUpdateEntity, OperationModel>()
            .ForMember(src => src.Type, o => o.MapFrom(x => ChangeType.Update.ToString()));
        CreateMap<VGAddEntity, OperationModel>()
            .ForMember(src => src.Type, o => o.MapFrom(x => ChangeType.Add.ToString()));
        CreateMap<VGDeleteEntity, OperationModel>()
            .ForMember(src => src.Type, o => o.MapFrom(x => ChangeType.Delete.ToString()));
    }
}
