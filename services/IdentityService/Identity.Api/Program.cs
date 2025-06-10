using Identity.Application;
using Identity.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

// Get connection string from configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

// Add Infrastructure layer with PostgreSQL configuration
builder.Services.AddInfrastructure(connectionString);

// Add Application layer
builder.Services.AddApplication();

// Add Controllers
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

