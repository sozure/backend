using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VGManager.AzureAdapter.Entities;
using VGManager.Services.Interfaces;

namespace VGManager.Api.UserProfile;

[Route("api/[controller]")]
[ApiController]
[EnableCors("_allowSpecificOrigins")]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpPost("Get", Name = "getprofile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProfileResponse>> GetAsync(
        [FromBody] ProfileRequest request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var profile = await _profileService.GetProfileAsync(request.Organization, request.PAT, cancellationToken);
            if (profile is null)
            {
                return Ok(new ProfileResponse()
                {
                    Profile = null!,
                    Status = AdapterStatus.Unknown
                });
            }
            return Ok(new ProfileResponse()
            {
                Profile = profile,
                Status = AdapterStatus.Success
            });
        }
        catch (Exception)
        {
            return Ok(new ProfileResponse()
            {
                Profile = null!,
                Status = AdapterStatus.Unknown
            });
        }

    }
}
