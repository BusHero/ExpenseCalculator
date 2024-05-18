using System.Security.Claims;
using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.Models;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer;

public sealed class CustomProfileService(
    UserManager<ApplicationUser> userManager,
    IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
    : ProfileService<ApplicationUser>(userManager, claimsFactory)
{
    protected override async Task GetProfileDataAsync(
        ProfileDataRequestContext context,
        ApplicationUser user)
    {
        var principal = await GetUserClaimsAsync(user);
        var id = (ClaimsIdentity)principal.Identity!;
        if (!string.IsNullOrEmpty(user.FavoriteColor))
        {
            id.AddClaim(new Claim("favorite_color", user.FavoriteColor));
        }

        context.AddRequestedClaims(principal.Claims);
    }
}
