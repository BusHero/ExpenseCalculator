// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.IdentityServer.Services;
using IdentityServerAspNetIdentity.Pages.Error;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServerAspNetIdentity.Pages.Home.Error;

[AllowAnonymous]
[SecurityHeaders]
public class Index : PageModel
{
    private readonly IIdentityServerInteractionService interaction;
    private readonly IWebHostEnvironment environment;
        
    public ViewModel View { get; set; } = new();
        
    public Index(IIdentityServerInteractionService interaction, IWebHostEnvironment environment)
    {
        this.interaction = interaction;
        this.environment = environment;
    }
        
    public async Task OnGet(string? errorId)
    {
        // retrieve error details from identityserver
        var message = await interaction.GetErrorContextAsync(errorId);
        if (message != null)
        {
            View.Error = message;

            if (!environment.IsDevelopment())
            {
                // only show in development
                message.ErrorDescription = null;
            }
        }
    }
}
