using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Net.Http.Headers;

namespace ExpenseManager.IntegrationTests;

[Trait("Category", "Integration")]
public class CreateTests
    : IClassFixture<MyWebFactory>
{
    private readonly MyWebFactory factory;
    private readonly HttpClient client;

    public CreateTests(MyWebFactory factory)
    {
        this.factory = factory;
        client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
        });
    }

    [Fact]
    public async Task Get_Returns200()
    {
        factory.WithUserAuthenticationStatus(true);
        var status = await client.GetAsync("/Create");

        status.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Post_UnauthenticatedUser_Returns401()
    {
        factory.WithUserAuthenticationStatus(false);
        var response = await client.GetAsync("/Create");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Unauthorized);
    }
    
    // This is unexpected
    [Fact]
    public async Task Post_NoAntiForgeryCookie_Returns302()
    {
        factory.WithUserAuthenticationStatus(true);
        var initResponse = await client.GetAsync("/Create");
        var antiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(initResponse);
        
        var content = new FormUrlEncodedContent([
            new KeyValuePair<string, string>(AntiForgeryTokenExtractor.AntiForgeryFieldName, antiForgeryValues.fieldValue),
            new KeyValuePair<string, string>("Expense", "bar"),
            new KeyValuePair<string, string>("Amount", "123.123"),
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
        factory.WithUserAuthenticationStatus(true);
        var initResponse = await client.GetAsync("/Create");
        var antiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(initResponse);
        
        var content = new FormUrlEncodedContent([
            new KeyValuePair<string, string>("Expense", "bar"),
            new KeyValuePair<string, string>("Amount", "123.123"),
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
        factory.WithUserAuthenticationStatus(true);
        var initResponse = await client.GetAsync("/Create");
        var antiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(initResponse);
        
        var content = new FormUrlEncodedContent([
            new KeyValuePair<string, string>(AntiForgeryTokenExtractor.AntiForgeryFieldName, antiForgeryValues.fieldValue),
            new KeyValuePair<string, string>("Expense", "bar"),
            new KeyValuePair<string, string>("Amount", "123.123"),
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
        factory.WithUserAuthenticationStatus(true);
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

    [Fact]
    public async Task Post_FailedValidationReturn400()
    {
        factory.WithUserAuthenticationStatus(true);
        var initResponse = await client.GetAsync("/Create");
        var antiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(initResponse);
        
        var content = new FormUrlEncodedContent([
            new(AntiForgeryTokenExtractor.AntiForgeryFieldName, antiForgeryValues.fieldValue),
            new("Expense", null),
            new("Amount", "123.123"),
        ]);

        var request = new HttpRequestMessage(HttpMethod.Post, "/Create");
        request.Headers.Add("Cookie", new CookieHeaderValue(AntiForgeryTokenExtractor.AntiForgeryCookieName, antiForgeryValues.cookieValue).ToString());
        request.Content = content;
        
        var response = await client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}