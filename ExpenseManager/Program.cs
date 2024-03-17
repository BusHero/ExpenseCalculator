using ExpenseManager;
using ExpensesManager.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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

builder.Services.AddTransient<IApplicationService, ApplicationService>();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SetDefaultCulture("en-US");
});

builder.Services.AddDefaultIdentity<ApplicationUser>(
        options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
        })
    .AddEntityFrameworkStores<ApplicationContext>();

builder.Services.AddTransient<UserManager<ApplicationUser>>();

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

app.MapRazorPages();

app.Run();

public partial class Program;