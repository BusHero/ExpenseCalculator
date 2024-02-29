using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseManager.IntegrationTests;

public class MyWebFactory: WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(x =>
        {
            x.AddAntiforgery(t =>
            {
                t.Cookie.Name = AntiForgeryTokenExtractor.AntiForgeryCookieName;
                t.FormFieldName = AntiForgeryTokenExtractor.AntiForgeryFieldName;
            });
        });
    }
}
