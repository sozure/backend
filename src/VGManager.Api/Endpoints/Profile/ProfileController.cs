using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Services.Profile;
using VGManager.Adapter.Models.Models;
using VGManager.Adapter.Models.StatusEnums;
using VGManager.Services.Interfaces;

namespace VGManager.Api.Endpoints.UserProfile;

[Route("api/[controller]")]
[ApiController]
[EnableCors("_allowSpecificOrigins")]
public class ProfileController(IProfileService profileService) : ControllerBase
{
    [HttpPost(Name = "getprofile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AdapterResponseModel<Profile>>> GetAsync(
        [FromBody] ProfileRequest request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var profile = await profileService.GetProfileAsync(request.Organization, request.PAT, cancellationToken);
            if (profile is null)
            {
                return Ok(new AdapterResponseModel<Profile>()
                {
                    Data = null!,
                    Status = AdapterStatus.Unknown
                });
            }
            return Ok(new AdapterResponseModel<Profile>()
            {
                Data = profile,
                Status = AdapterStatus.Success
            });
        }
        catch (Exception)
        {
            return Ok(new AdapterResponseModel<Profile>()
            {
                Data = null!,
                Status = AdapterStatus.Unknown
            });
        }

    }
}
