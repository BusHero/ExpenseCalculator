using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace ExpenseManager.IntegrationTests;

public class CreateTests
    : IClassFixture<MyWebFactory>
{
    private readonly HttpClient client;

    public CreateTests(MyWebFactory factory)
    {
        client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
        });
    }

    [Fact]
    public async Task Get_Returns200()
    {
        var status = await client.GetAsync("/Create");

        status.EnsureSuccessStatusCode();
    }
    
    // This is unexpected
    [Fact]
    public async Task Post_NoAntiForgeryCookie_Returns302()
    {
        var initResponse = await client.GetAsync("/Create");
        var antiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(initResponse);
        
        var content = new FormUrlEncodedContent([
            new(AntiForgeryTokenExtractor.AntiForgeryFieldName, antiForgeryValues.fieldValue),
            new("Expense", "bar"),
            new("Amount", "123.123"),
        ]);

        var request = new HttpRequestMessage(HttpMethod.Post, "/Create");
        request.Content = content;
        
        var response = await client.SendAsync(request);

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Redirect);
    }
    
    [Fact]
    public async Task Post_NoAntiForgeryField_Returns400()
    {
        var initResponse = await client.GetAsync("/Create");
        var antiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(initResponse);
        
        var content = new FormUrlEncodedContent([
            new("Expense", "bar"),
            new("Amount", "123.123"),
        ]);

        var request = new HttpRequestMessage(HttpMethod.Post, "/Create");
        request.Headers.Add("Cookie", new CookieHeaderValue(AntiForgeryTokenExtractor.AntiForgeryCookieName, antiForgeryValues.cookieValue).ToString());
        request.Content = content;
        
        var response = await client.SendAsync(request);

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_ReturnRedirect()
    {
        var initResponse = await client.GetAsync("/Create");
        var antiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(initResponse);
        
        var content = new FormUrlEncodedContent([
            new(AntiForgeryTokenExtractor.AntiForgeryFieldName, antiForgeryValues.fieldValue),
            new("Expense", "bar"),
            new("Amount", "123.123"),
        ]);

        var request = new HttpRequestMessage(HttpMethod.Post, "/Create");
        request.Headers.Add("Cookie", new CookieHeaderValue(AntiForgeryTokenExtractor.AntiForgeryCookieName, antiForgeryValues.cookieValue).ToString());
        request.Content = content;
        
        var response = await client.SendAsync(request);

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Redirect);
    }


    [Fact]
    public async Task Post_RedirectToIndex()
    {
        var initResponse = await client.GetAsync("/Create");
        var antiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(initResponse);
        
        var content = new FormUrlEncodedContent([
            new(AntiForgeryTokenExtractor.AntiForgeryFieldName, antiForgeryValues.fieldValue),
            new("Expense", "bar"),
            new("Amount", "123.123"),
        ]);

        var request = new HttpRequestMessage(HttpMethod.Post, "/Create");
        request.Headers.Add("Cookie", new CookieHeaderValue(AntiForgeryTokenExtractor.AntiForgeryCookieName, antiForgeryValues.cookieValue).ToString());
        request.Content = content;
        
        var response = await client.SendAsync(request);

        response
            .Headers
            .Location!
            .OriginalString
            .Should()
            .Be("/");
    }
}

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

public static class AntiForgeryTokenExtractor
{
    public async static Task<(string fieldValue, string cookieValue)> ExtractAntiForgeryValues(HttpResponseMessage response)
    {
        var cookie = ExtractAntiForgeryCookieValueFrom(response);
        var token = ExtractAntiForgeryToken(await response.Content.ReadAsStringAsync());
    
        return (fieldValue: token, cookieValue: cookie);
    }
    
    public static string AntiForgeryFieldName { get; } = "AntiForgeryTokenField";
    
    public static string AntiForgeryCookieName { get; } = "AntiForgeryTokenCookie";
    
    private static string ExtractAntiForgeryCookieValueFrom(HttpResponseMessage response)
    {
        var antiForgeryCookie = response.Headers.GetValues("Set-Cookie")
            .FirstOrDefault(x => x.Contains(AntiForgeryCookieName));

        if (antiForgeryCookie is null)
            throw new ArgumentException($"Cookie '{AntiForgeryCookieName}' not found in HTTP response", nameof(response));

        var antiForgeryCookieValue = SetCookieHeaderValue.Parse(antiForgeryCookie).Value.ToString();

        return antiForgeryCookieValue;
    }
    
    private static string ExtractAntiForgeryToken(string htmlBody)
    {
         var requestVerificationTokenMatch =
             Regex.Match(htmlBody, $"""\<input name="{AntiForgeryFieldName}" type="hidden" value="([^"]+)" \/\>""");
     
         if (requestVerificationTokenMatch.Success)
             return requestVerificationTokenMatch.Groups[1].Captures[0].Value;
     
         throw new ArgumentException($"Anti forgery token '{AntiForgeryFieldName}' not found in HTML", nameof(htmlBody));
     }
    
}
