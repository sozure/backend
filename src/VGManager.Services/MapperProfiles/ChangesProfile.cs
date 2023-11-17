using AutoMapper;
using VGManager.Entities;
using VGManager.Services.Models.Changes;

namespace VGManager.Services.MapperProfiles;

public class ChangesProfile : Profile
{
    public ChangesProfile()
    {
        CreateMap<OperationEntity, OperationModel>()
            .ForMember(src => src.Type, o => o.MapFrom(x => ChangeType.None.ToString()));
        CreateMap<EditionEntity, OperationModel>()
            .ForMember(src => src.Type, o => o.MapFrom(x => ChangeType.Edition.ToString()));
        CreateMap<AdditionEntity, OperationModel>()
            .ForMember(src => src.Type, o => o.MapFrom(x => ChangeType.Addition.ToString()));
        CreateMap<DeletionEntity, OperationModel>()
            .ForMember(src => src.Type, o => o.MapFrom(x => ChangeType.Deletion.ToString()));
    }
}
