using AutoMapper;
using Humanizer;
using VGManager.Repositories.Interfaces;
using VGManager.Services.Interfaces;
using VGManager.Services.Models.Changes;

namespace VGManager.Services;

public class ChangesService: IChangesService
{
    private readonly IAdditionColdRepository _additionColdRepository;
    private readonly IEditionColdRepository _editionColdRepository;
    private readonly IDeletionColdRepository _deletionColdRepository;
    private readonly IMapper _mapper;

    public ChangesService(
        IAdditionColdRepository additionColdRepository,
        IEditionColdRepository editionColdRepository,
        IDeletionColdRepository deletionColdRepository,
        IMapper mapper
    )
    {
        _additionColdRepository = additionColdRepository;
        _editionColdRepository = editionColdRepository;
        _deletionColdRepository = deletionColdRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OperationModel>> GetByDateAsync(
        DateTime from, 
        DateTime to, 
        ChangeType[] changeTypes, 
        CancellationToken cancellationToken = default
        )
    {
        var result = new List<OperationModel>();
        foreach(var changeType in changeTypes)
        {
            switch(changeType)
            {
                case ChangeType.Addition:
                    var additions = _mapper.Map<IEnumerable<AdditionModel>>(
                        await _additionColdRepository.GetByDateAsync(from, to, cancellationToken)
                        );
                    result.AddRange(additions);
                    break;
                case ChangeType.Edition:
                    var editions = _mapper.Map<IEnumerable<EditionModel>>(
                        await _editionColdRepository.GetByDateAsync(from, to, cancellationToken)
                        );
                    result.AddRange(editions);
                    break;
                case ChangeType.Deletion:
                    var deletions = _mapper.Map<IEnumerable<DeletionModel>>(
                        await _deletionColdRepository.GetByDateAsync(from, to, cancellationToken)
                        );
                    result.AddRange(deletions);
                    break;
                default:
                    throw new InvalidOperationException($"ChangeType does not exist: {nameof(changeType)}");
            }
        }
        return result;
    }

    public async Task<IEnumerable<OperationModel>> GetByMaxLimitAsync(
        int limit, 
        ChangeType[] changeTypes, 
        CancellationToken cancellationToken = default
        )
    {
        var result = new List<OperationModel>();
        foreach (var changeType in changeTypes)
        {
            switch (changeType)
            {
                case ChangeType.Addition:
                    var additions = _mapper.Map<IEnumerable<AdditionModel>>(
                        await _additionColdRepository.GetAllAsync(cancellationToken)
                        );
                    result.AddRange(additions);
                    break;
                case ChangeType.Edition:
                    var editions = _mapper.Map<IEnumerable<EditionModel>>(
                        await _editionColdRepository.GetAllAsync(cancellationToken)
                        );
                    result.AddRange(editions);
                    break;
                case ChangeType.Deletion:
                    var deletions = _mapper.Map<IEnumerable<DeletionModel>>(
                        await _deletionColdRepository.GetAllAsync(cancellationToken)
                        );
                    result.AddRange(deletions);
                    break;
                default:
                    throw new InvalidOperationException($"ChangeType does not exist: {nameof(changeType)}");
            }
        }
        var sortedResult = result.OrderBy(entity => entity.Date);
        return sortedResult.Take(limit);
    }
}
