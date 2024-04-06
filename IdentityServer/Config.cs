using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace IdentityServerAspNetIdentity;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
    [
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new()
        {
            Name = "verification",
            UserClaims = new List<string>
            {
                JwtClaimTypes.Email,
                JwtClaimTypes.EmailVerified,
            },
        },
        new("color", ["favorite_color",]),
    ];

    public static IEnumerable<ApiScope> ApiScopes =>
    [
        new(name: "api1", displayName: "My API"),
    ];

    public static IEnumerable<Client> Clients =>
    [
        new()
        {
            ClientId = "client",

            AllowedGrantTypes = GrantTypes.ClientCredentials,

            ClientSecrets =
            {
                new Secret("secret".Sha256()),
            },

            AllowedScopes = { "api1", },
        },
        new()
        {
            ClientId = "web",
            ClientSecrets = { new Secret("secret".Sha256()) },

            AllowedGrantTypes = GrantTypes.Code,

            // where to redirect to after login
            RedirectUris = { "https://localhost:5002/signin-oidc", },

            // where to redirect to after logout
            PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc", },

            AllowOfflineAccess = true,

            AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                "verification",
                "api1",
                "color",
            },
        },
    ];
}
