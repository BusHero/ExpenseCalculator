using System.Text.Json;
using IdentityModel.Client;

var options = new JsonSerializerOptions { WriteIndented = true, };
var httpClient = new HttpClient();
var disco = await httpClient.GetDiscoveryDocumentAsync("https://localhost:5001");
if (disco.IsError)
{
    Console.WriteLine(disco.Error);
    return;
}

var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(new()
{
    Address = disco.TokenEndpoint,
    ClientId = "client",
    ClientSecret = "secret",
    Scope = "api1",
});

if (tokenResponse.IsError)
{
    Console.WriteLine(tokenResponse.Error);
    Console.WriteLine(tokenResponse.ErrorDescription);
}

var apiClient = new HttpClient();
apiClient.SetBearerToken(tokenResponse.AccessToken!);

var response = await apiClient.GetAsync("https://localhost:6001/identity");
if (!response.IsSuccessStatusCode)
{
    Console.WriteLine(response.StatusCode);
}
else
{
    var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
    Console.WriteLine(JsonSerializer.Serialize(doc, options));
}