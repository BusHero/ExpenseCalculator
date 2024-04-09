// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages.Account.Logout;

[SecurityHeaders]
[AllowAnonymous]
public class LoggedOut : PageModel
{
    private readonly IIdentityServerInteractionService interactionService;

    public LoggedOutViewModel View { get; set; } = default!;

    public LoggedOut(IIdentityServerInteractionService interactionService)
    {
        this.interactionService = interactionService;
    }

    public async Task OnGet(string? logoutId)
    {
        // get context information (client name, post logout redirect URI and iframe for federated signout)
        var logout = await interactionService.GetLogoutContextAsync(logoutId);

        View = new()
        {
            AutomaticRedirectAfterSignOut = LogoutOptions.AutomaticRedirectAfterSignOut,
            PostLogoutRedirectUri = logout.PostLogoutRedirectUri,
            ClientName = string.IsNullOrEmpty(logout.ClientName) ? logout.ClientId : logout.ClientName,
            SignOutIframeUrl = logout.SignOutIFrameUrl,
        };
    }
}
