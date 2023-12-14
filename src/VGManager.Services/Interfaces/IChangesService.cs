using VGManager.Services.Models.Changes.Requests;
using VGManager.Services.Models.Changes.Responses;

namespace VGManager.Services.Interfaces;

public interface IChangesService
{
    Task<IEnumerable<VGOperationModel>> GetAsync(
        VGRequestModel model,
        CancellationToken cancellationToken = default
        );

    Task<IEnumerable<SecretOperationModel>> GetAsync(
    SecretRequestModel model,
    CancellationToken cancellationToken = default
    );

    Task<IEnumerable<KVOperationModel>> GetAsync(
    KVRequestModel model,
    CancellationToken cancellationToken = default
    );
}
