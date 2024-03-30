using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
    [
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new()
        {
            Name = "verification",
            UserClaims =
            [
                JwtClaimTypes.Email,
                JwtClaimTypes.EmailVerified,
            ],
        },
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
            [
                new("secret".Sha256()),
            ],
            AllowedScopes = { "api1", },
        },
        new()
        {
            ClientId = "web",
            ClientSecrets = { new Secret("secret".Sha256()), },
            AllowedGrantTypes = GrantTypes.Code,
            RedirectUris = { "https://localhost:5002/signin-oidc", },
            PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc", },
            AllowOfflineAccess = true,
            AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                "verification",
                "api1",
            },
        },
    ];
}
