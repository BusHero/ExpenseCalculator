using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("Cookies")
    .AddOpenIdConnect(
        "oidc",
        options =>
        {
            options.Authority = "https://localhost:5001";

            options.ClientId = "web";
            options.ClientSecret = "secret";
            options.ResponseType = "code";

            options.Scope.Clear();
            options.Scope.Add("openid");
            options.Scope.Add("profile");
            options.Scope.Add("api1");
            options.Scope.Add("offline_access");
            options.Scope.Add("verification");
            options.Scope.Add("color");
            options.ClaimActions.MapJsonKey("email_verified", "email_verified");
            options.ClaimActions.MapUniqueJsonKey("favorite_color", "favorite_color");
            options.GetClaimsFromUserInfoEndpoint = true;

            options.MapInboundClaims = false;// Don't rename claim types

            options.SaveTokens = true;
        })
    ;
;

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages().RequireAuthorization();

app.Run();
