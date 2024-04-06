using ExpenseManager;
using ExpensesManager.DataAccess;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        x =>
        {
            var assemblyName = typeof(ExpenseManager.DataAccess.Migrations.Marker)
                .Assembly
                .GetName()
                .Name;
            x.MigrationsAssembly(assemblyName);
        });
});

builder.Services.AddAuthentication(
        x =>
        {
            x.DefaultScheme = "Cookies";
            x.DefaultChallengeScheme = "oidc";
        })
    .AddCookie("Cookies")
    .AddOpenIdConnect(
        "oidc", 
        options =>
        {
            options.Authority = builder.Configuration["IdentityServer:Authority"];
    
            options.ClientId = builder.Configuration["IdentityServer:ClientId"];
            options.ClientSecret = builder.Configuration["IdentityServer:ClientSecret"];
            options.ResponseType = "code";
    
            options.Scope.Clear();
            options.Scope.Add("openid");
            options.Scope.Add("profile");
            options.Scope.Add("offline_access");
            options.Scope.Add("verification");
            options.ClaimActions.MapJsonKey("email_verified", "email_verified");
            options.ClaimActions.MapUniqueJsonKey("favorite_color", "favorite_color");
            options.GetClaimsFromUserInfoEndpoint = true;
    
            options.MapInboundClaims = false;// Don't rename claim types
    
            options.SaveTokens = true;
        });

builder.Services.AddTransient<IApplicationService, ApplicationService>();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SetDefaultCulture("en-US");
});

var app = builder
    .Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    await context.Database.EnsureCreatedAsync();
}

if (!app.Environment.IsDevelopment()){
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseRequestLocalization();

app.MapRazorPages().RequireAuthorization();

app.Run();

public partial class Program;