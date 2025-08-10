var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // http://localhost:5244/openapi/v1.json
}

Console.WriteLine("Bookify API is starting...");

// app.UseHttpsRedirection();

app.MapControllers();

app.Run();
