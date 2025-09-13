using Bookify.Api.Extensions;
using Bookify.Application;
using Bookify.Infrastructure;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // http://localhost:5000/openapi/v1.json
    app.MapScalarApiReference(); // http://localhost:5000/scalar/v1
    app.ApplyMigrations();
}

Console.WriteLine("Bookify API is starting...");

// app.UseHttpsRedirection();

app.MapControllers();

app.Run();
