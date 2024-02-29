using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Net.Http.Headers;

namespace ExpenseManager.IntegrationTests;

[Trait("Category", "Integration")]
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

    [Fact]
    public async Task Post_FailedValidationReturn400()
    {
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