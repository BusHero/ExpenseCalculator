using ExpenseManager.Domain;
using ExpenseManager.LocalDevelopment;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddSingleton<IExpenseStorage, InMemoryExpensesStorage>();

var app = builder.Build();

if (!app.Environment.IsDevelopment()){
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

public partial class Program;