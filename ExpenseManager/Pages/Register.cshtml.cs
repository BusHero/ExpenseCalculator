using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace ExpenseManager.Pages;

[AllowAnonymous]
public class Register(
    IOptions<RegisterOptions> options) : PageModel
{
    private readonly RegisterOptions options = options.Value;
    
    public IActionResult OnGet()
    {
        var uri = new UriBuilder(options.RegisterUri!)
        {
            Query = $"returnUrl={Url.PageLink("Index")}"
        }.Uri;
        
        return Redirect(uri.ToString());
    }
}

public sealed record RegisterOptions
{
    public const string Register = "Register";
    
    public Uri RegisterUri { get; init; } = null!;
}
