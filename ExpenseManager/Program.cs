using ExpenseManager;
using ExpenseManager.Domain;
using ExpenseManager.LocalDevelopment;
using ExpensesManager.DataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddSingleton<IExpenseStorage, InMemoryExpensesStorage>();

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationContext>();

var app = builder.Build();

if (!app.Environment.IsDevelopment()){
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

public partial class Program;