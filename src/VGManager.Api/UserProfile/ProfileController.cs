using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VGManager.Api.Projects.Responses;
using VGManager.Api.Projects;
using VGManager.Services;
using VGManager.Services.Interfaces;
using VGManager.Services.Models.Projects;

namespace VGManager.Api.UserProfile;

[Route("api/[controller]")]
[ApiController]
[EnableCors("_allowSpecificOrigins")]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;
    private readonly IMapper _mapper;

    public ProfileController(IProfileService profileService, IMapper mapper)
    {
        _profileService = profileService;
        _mapper = mapper;
    }

    [HttpPost("Get", Name = "getprofile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProjectsResponse>> GetAsync(
        [FromBody] ProfileRequest request,
        CancellationToken cancellationToken
    )
    {
        var profile = await _profileService.GetProfile(request.Organization, request.PAT, cancellationToken);
        return Ok(profile);
    }
}
