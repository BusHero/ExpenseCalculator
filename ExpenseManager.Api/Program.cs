using ExpenseManager.Api;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IExpenseStorage, InMemoryExpensesStorage>();

var app = builder.Build();

if (app.Environment.IsDevelopment()){
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/weatherforecast",
    () => Results.Ok())
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.MapPost(
    "/expense/create", 
    (IExpenseStorage storage, [FromBody] Expense expense) =>
    {
        storage.Add(expense);
        
        return Results.Ok();
    })
    .WithOpenApi();

app.MapGet(
        "/expense/all",
        (IExpenseStorage storage) => storage.GetAll())
    .WithOpenApi(); 

app.Run();

public abstract partial class Program;