using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExpenseManager.Pages;

public class UserInfo : PageModel
{
    public async Task OnGet()
    {
        var client = new HttpClient();
        var disco = await client
            .GetDiscoveryDocumentAsync("https://localhost:5001");
        var token = await HttpContext.GetTokenAsync("access_token");
        var response2 = await client.GetUserInfoAsync(new UserInfoRequest
        {
            Address = disco.UserInfoEndpoint,
            Token = token,
        });
    }
}