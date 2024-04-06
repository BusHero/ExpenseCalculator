using System.Security.Claims;

namespace Api;

public sealed class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.Authority = "https://localhost:5001";
                options.TokenValidationParameters.ValidateAudience = false;
            });
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("ApiScope",
                policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "api1");
                });
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.MapGet("identity", (ClaimsPrincipal user) => user.Claims.Select(c => new { c.Type, c.Value }))
            .WithOpenApi()
            .RequireAuthorization("ApiScope");

        app.Run();
    }
}