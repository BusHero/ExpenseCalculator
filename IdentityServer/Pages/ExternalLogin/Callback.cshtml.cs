using System.Security.Claims;
using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;
using IdentityModel;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServerAspNetIdentity.Pages.ExternalLogin;

[AllowAnonymous]
[SecurityHeaders]
public class Callback : PageModel
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly IIdentityServerInteractionService interaction;
    private readonly ILogger<Callback> logger;
    private readonly IEventService events;

    public Callback(
        IIdentityServerInteractionService interaction,
        IEventService events,
        ILogger<Callback> logger,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.interaction = interaction;
        this.logger = logger;
        this.events = events;
    }

    public async Task<IActionResult> OnGet()
    {
        var result = await HttpContext
            .AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
        if (result.Succeeded != true)
        {
            throw new InvalidOperationException($"External authentication error: {result.Failure}");
        }

        var externalUser = result.Principal
                           ?? throw new InvalidOperationException("External authentication produced a null Principal");

        if (logger.IsEnabled(LogLevel.Debug))
        {
            var externalClaims = externalUser
                .Claims
                .Select(c => $"{c.Type}: {c.Value}");
            logger.ExternalClaims(externalClaims);
        }

        var userIdClaim = externalUser.FindFirst(JwtClaimTypes.Subject)
                          ?? externalUser.FindFirst(ClaimTypes.NameIdentifier)
                          ?? throw new InvalidOperationException("Unknown userid");

        var provider = result.Properties.Items["scheme"]
                       ?? throw new InvalidOperationException("Null scheme in authentication properties");
        var providerUserId = userIdClaim.Value;

        var user = await userManager
            .FindByLoginAsync(provider, providerUserId) ?? await AutoProvisionUserAsync(provider, providerUserId, externalUser.Claims);

        var additionalLocalClaims = new List<Claim>();
        var localSignInProps = new AuthenticationProperties();
        CaptureExternalLoginContext(result, additionalLocalClaims, localSignInProps);

        await signInManager.SignInWithClaimsAsync(user, localSignInProps, additionalLocalClaims);

        await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

        var returnUrl = result.Properties.Items["returnUrl"] ?? "~/";

        var context = await interaction.GetAuthorizationContextAsync(returnUrl);
        await events.RaiseAsync(
            new UserLoginSuccessEvent(
                provider,
                providerUserId,
                user.Id,
                user.UserName,
                true,
                context?.Client.ClientId));
        Telemetry.Metrics.UserLogin(context?.Client.ClientId, provider);

        if (context == null)
        {
            return Redirect(returnUrl);
        }
        return context.IsNativeClient()
            ? this.LoadingPage(returnUrl)
            : Redirect(returnUrl);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Performance",
        "CA1851:Possible multiple enumerations of 'IEnumerable' collection",
        Justification = "<Pending>")]
    private async Task<ApplicationUser> AutoProvisionUserAsync(
        string provider,
        string providerUserId,
        IEnumerable<Claim> claims)
    {
        var sub = Guid.NewGuid().ToString();

        var user = new ApplicationUser
        {
            Id = sub,
            UserName = sub,// don't need a username, since the user will be using an external provider to login
            FavoriteColor = "red",
        };

        // email
        var enumerable = claims as Claim[] ?? claims.ToArray();
        var email = enumerable.FirstOrDefault(x => x.Type == JwtClaimTypes.Email)?.Value ??
                    enumerable.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        if (email != null)
        {
            user.Email = email;
        }

        // create a list of claims that we want to transfer into our store
        var filtered = new List<Claim>();

        // user's display name
        var name = enumerable.FirstOrDefault(x => x.Type == JwtClaimTypes.Name)?.Value ??
                   enumerable.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        if (name != null)
        {
            filtered.Add(new Claim(JwtClaimTypes.Name, name));
        }
        else
        {
            var first = enumerable.FirstOrDefault(x => x.Type == JwtClaimTypes.GivenName)?.Value ??
                        enumerable.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
            var last = enumerable.FirstOrDefault(x => x.Type == JwtClaimTypes.FamilyName)?.Value ??
                       enumerable.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value;
            if (first != null && last != null)
            {
                filtered.Add(new Claim(JwtClaimTypes.Name, first + " " + last));
            }
            else if (first != null)
            {
                filtered.Add(new Claim(JwtClaimTypes.Name, first));
            }
            else if (last != null)
            {
                filtered.Add(new Claim(JwtClaimTypes.Name, last));
            }
        }

        var identityResult = await userManager.CreateAsync(user);
        if (!identityResult.Succeeded)
        {
            throw new InvalidOperationException(identityResult.Errors.First().Description);
        }

        if (filtered.Count != 0)
        {
            identityResult = await userManager.AddClaimsAsync(user, filtered);
            if (!identityResult.Succeeded)
            {
                throw new InvalidOperationException(identityResult.Errors.First().Description);
            }
        }

        identityResult = await userManager.AddLoginAsync(
            user,
            new(provider, providerUserId, provider));
        if (!identityResult.Succeeded)
        {
            throw new InvalidOperationException(identityResult.Errors.First().Description);
        }

        return user;
    }

    // if the external login is OIDC-based, there are certain things we need to preserve to make logout work
    // this will be different for WS-Fed, SAML2p or other protocols
    private static void CaptureExternalLoginContext(
        AuthenticateResult externalResult,
        List<Claim> localClaims,
        AuthenticationProperties localSignInProps)
    {
        ArgumentNullException.ThrowIfNull(
            externalResult.Principal,
            nameof(externalResult.Principal));

        // capture the idp used to login, so the session knows where the user came from
        localClaims.Add(new(
            JwtClaimTypes.IdentityProvider,
            externalResult.Properties?.Items["scheme"] ?? "unknown identity provider"));

        // if the external system sent a session id claim, copy it over
        // so we can use it for single sign-out
        var sid = externalResult
            .Principal
            .Claims
            .FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
        if (sid != null)
        {
            localClaims.Add(new(JwtClaimTypes.SessionId, sid.Value));
        }

        // if the external provider issued an id_token, we'll keep it for sign out
        var idToken = externalResult.Properties?.GetTokenValue("id_token");
        if (idToken != null)
        {
            localSignInProps.StoreTokens(
            [
                new()
                {
                    Name = "id_token",
                    Value = idToken,
                },
            ]);
        }
    }
}
