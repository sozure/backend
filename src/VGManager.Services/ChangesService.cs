using AutoMapper;
using VGManager.Repositories.Interfaces.VGRepositories;
using VGManager.Services.Interfaces;
using VGManager.Services.Models.Changes;

namespace VGManager.Services;

public class ChangesService : IChangesService
{
    private readonly IVGAddColdRepository _additionColdRepository;
    private readonly IVGUpdateColdRepository _editionColdRepository;
    private readonly IVGDeleteColdRepository _deletionColdRepository;
    private readonly IMapper _mapper;

    public ChangesService(
        IVGAddColdRepository additionColdRepository,
        IVGUpdateColdRepository editionColdRepository,
        IVGDeleteColdRepository deletionColdRepository,
        IMapper mapper
    )
    {
        _additionColdRepository = additionColdRepository;
        _editionColdRepository = editionColdRepository;
        _deletionColdRepository = deletionColdRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OperationModel>> GetAsync(
        RequestModel model,
        CancellationToken cancellationToken = default
        )
    {
        var result = new List<OperationModel>();
        var organization = model.Organization;
        var project = model.Project;
        var user = model.User;
        var from = model.From;
        var to = model.To;
        foreach (var changeType in model.ChangeTypes)
        {
            switch (changeType)
            {
                case ChangeType.Add:
                    var addEntities = user is null ?
                        await _additionColdRepository.GetAsync(organization, project, from, to, cancellationToken) :
                        await _additionColdRepository.GetAsync(organization, project, user, from, to, cancellationToken);
                    result.AddRange(_mapper.Map<IEnumerable<OperationModel>>(addEntities));
                    break;
                case ChangeType.Update:
                    var updateEntities = user is null ?
                        await _editionColdRepository.GetAsync(organization, project, from, to, cancellationToken) :
                        await _editionColdRepository.GetAsync(organization, project, user, from, to, cancellationToken);
                    result.AddRange(_mapper.Map<IEnumerable<OperationModel>>(updateEntities));
                    break;
                case ChangeType.Delete:
                    var deleteEntities = user is null ?
                        await _deletionColdRepository.GetAsync(organization, project, from, to, cancellationToken) :
                        await _deletionColdRepository.GetAsync(organization, project, user, from, to, cancellationToken);
                    result.AddRange(_mapper.Map<IEnumerable<OperationModel>>(deleteEntities));
                    break;
                default:
                    throw new InvalidOperationException($"ChangeType does not exist: {nameof(changeType)}");
            }
        }
        var sortedResult = result.OrderByDescending(entity => entity.Date);
        return sortedResult.Take(model.Limit);
    }
}
