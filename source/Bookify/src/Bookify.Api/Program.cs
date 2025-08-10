using Bookify.Application;
using Bookify.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // http://localhost:5000/openapi/v1.json
    app.MapScalarApiReference(); // http://localhost:5000/scalar/v1
}

Console.WriteLine("Bookify API is starting...");

// app.UseHttpsRedirection();

app.MapControllers();

app.Run();
