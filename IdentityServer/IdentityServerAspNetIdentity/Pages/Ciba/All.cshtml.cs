// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServerAspNetIdentity.Pages.Ciba;

[SecurityHeaders]
[Authorize]
public class AllModel : PageModel
{
    public IEnumerable<BackchannelUserLoginRequest> Logins { get; set; } = default!;

    private readonly IBackchannelAuthenticationInteractionService backchannelAuthenticationInteraction;

    public AllModel(IBackchannelAuthenticationInteractionService backchannelAuthenticationInteractionService)
    {
        backchannelAuthenticationInteraction = backchannelAuthenticationInteractionService;
    }

    public async Task OnGet()
    {
        Logins = await backchannelAuthenticationInteraction.GetPendingLoginRequestsForCurrentUserAsync();
    }
}