using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VGManager.Adapter.Models.Models;
using VGManager.Adapter.Models.StatusEnums;
using VGManager.Api.Common;
using VGManager.Services.Interfaces;
using AzureProfile = Microsoft.VisualStudio.Services.Profile.Profile;

namespace VGManager.Api.Handlers.Profile;

public static class ProfileHandler
{
    [ExcludeFromCodeCoverage]
    public static RouteGroupBuilder MapProfileHandler(this RouteGroupBuilder builder)
    {
        var group = builder.MapGroup("/profile");

        group.MapPost("/", GetProfileAsync)
            .WithName("GetProfile");

        return builder;
    }

    public static async Task<Ok<AdapterResponseModel<AzureProfile>>> GetProfileAsync(
        [FromBody] BasicRequest request,
        [FromServices] IProfileService profileService,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var profile = await profileService.GetProfileAsync(request.Organization, request.PAT, cancellationToken);
            if (profile is null)
            {
                return TypedResults.Ok(new AdapterResponseModel<AzureProfile>()
                {
                    Data = null!,
                    Status = AdapterStatus.Unknown
                });
            }
            return TypedResults.Ok(new AdapterResponseModel<AzureProfile>()
            {
                Data = profile,
                Status = AdapterStatus.Success
            });
        }
        catch (Exception)
        {
            return TypedResults.Ok(new AdapterResponseModel<AzureProfile>()
            {
                Data = null!,
                Status = AdapterStatus.Unknown
            });
        }
    }
}
