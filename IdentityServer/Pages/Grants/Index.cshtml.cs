// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages.Grants;

[SecurityHeaders]
[Authorize]
public class Index : PageModel
{
    private readonly IIdentityServerInteractionService interaction;
    private readonly IClientStore clients;
    private readonly IResourceStore _resources;
    private readonly IEventService events;

    public Index(IIdentityServerInteractionService interaction,
        IClientStore clients,
        IResourceStore resources,
        IEventService events)
    {
        this.interaction = interaction;
        this.clients = clients;
        _resources = resources;
        this.events = events;
    }

    public ViewModel View { get; set; } = default!;
        
    public async Task OnGet()
    {
        var grants = await interaction.GetAllUserGrantsAsync();

        var list = new List<GrantViewModel>();
        foreach (var grant in grants)
        {
            var client = await clients.FindClientByIdAsync(grant.ClientId);
            if (client == null)
            {
                continue;
            }
            var resources = await _resources.FindResourcesByScopeAsync(grant.Scopes);

            var item = new GrantViewModel()
            {
                ClientId = client.ClientId,
                ClientName = client.ClientName ?? client.ClientId,
                ClientLogoUrl = client.LogoUri,
                ClientUrl = client.ClientUri,
                Description = grant.Description,
                Created = grant.CreationTime, 
                Expires = grant.Expiration,
                IdentityGrantNames = resources.IdentityResources.Select(x => x.DisplayName ?? x.Name).ToArray(),
                ApiGrantNames = resources.ApiScopes.Select(x => x.DisplayName ?? x.Name).ToArray(),
            };

            list.Add(item);
        }

        View = new()
        {
            Grants = list,
        };
    }

    [BindProperty]
    public string? ClientId { get; set; }

    public async Task<IActionResult> OnPost()
    {
        await interaction.RevokeUserConsentAsync(ClientId);
        await events.RaiseAsync(new GrantsRevokedEvent(User.GetSubjectId(), ClientId));
        Telemetry.Metrics.GrantsRevoked(ClientId);

        return RedirectToPage("/Grants/Index");
    }
}
