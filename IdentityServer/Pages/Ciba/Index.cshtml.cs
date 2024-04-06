// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServerAspNetIdentity.Pages.Ciba;

[AllowAnonymous]
[SecurityHeaders]
public class IndexModel : PageModel
{
    public BackchannelUserLoginRequest LoginRequest { get; set; } = default!;

    private readonly IBackchannelAuthenticationInteractionService backchannelAuthenticationInteraction;
    private readonly ILogger<IndexModel> logger;

    public IndexModel(IBackchannelAuthenticationInteractionService backchannelAuthenticationInteractionService, ILogger<IndexModel> logger)
    {
        backchannelAuthenticationInteraction = backchannelAuthenticationInteractionService;
        this.logger = logger;
    }

    public async Task<IActionResult> OnGet(string id)
    {
        var result = await backchannelAuthenticationInteraction.GetLoginRequestByInternalIdAsync(id);
        if (result == null)
        {
            logger.InvalidBackchannelLoginId(id);
            return RedirectToPage("/Home/Error/Index");
        }
        LoginRequest = result;

        return Page();
    }
}