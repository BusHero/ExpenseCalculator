using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServerAspNetIdentity.Pages.ExternalLogin;

[AllowAnonymous]
[SecurityHeaders]
public class Challenge : PageModel
{
    private readonly IIdentityServerInteractionService interactionService;

    public Challenge(IIdentityServerInteractionService interactionService)
    {
        this.interactionService = interactionService;
    }
        
    public IActionResult OnGet(string scheme, string? returnUrl)
    {
        if (string.IsNullOrEmpty(returnUrl)) returnUrl = "~/";
        if (Url.IsLocalUrl(returnUrl) == false && interactionService.IsValidReturnUrl(returnUrl) == false)
        {
            throw new ArgumentException("invalid return URL");
        }
            
        var props = new AuthenticationProperties
        {
            RedirectUri = Url.Page("/externallogin/callback"),
                
            Items =
            {
                { "returnUrl", returnUrl }, 
                { "scheme", scheme },
            },
        };

        return Challenge(props, scheme);
    }
}
