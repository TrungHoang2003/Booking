using BuildingBlocks.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add minimal API services with CORS
builder.Services.AddMinimalApiServicesWithCors();

var app = builder.Build();

// Configure minimal pipeline with CORS and run
app.UseMinimalApiPipeline()
   .RunMinimal();
