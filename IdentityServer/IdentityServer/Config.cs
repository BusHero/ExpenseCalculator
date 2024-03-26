using Duende.IdentityServer.Models;

namespace IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
    [
        new IdentityResources.OpenId(),
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
    ];
}
