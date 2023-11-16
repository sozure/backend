using AutoMapper;
using VGManager.Entities;
using VGManager.Services.Models.Changes;

namespace VGManager.Services.MapperProfiles;

public class ChangesProfile : Profile
{
    public ChangesProfile()
    {
        CreateMap<OperationEntity, OperationModel>();
        CreateMap<EditionEntity, EditionModel>();
        CreateMap<DeletionEntity, DeletionModel>();
        CreateMap<AdditionEntity, AdditionModel>();
    }
}
