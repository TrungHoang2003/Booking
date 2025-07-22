using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Property.Infrastructure.DbHelper;
using Property.Infrastructure.Repositories;
using Serilog;
using BuildingBlocks.Interfaces;

namespace Property.Infrastructure;

public static class PropertyInfrastructureDi
{
   public static void AddInfrastructure(this IServiceCollection services, string? connectionString)
   {
      Log.Information("Using connection string: {ConnectionString}", connectionString);

      try
      {
         services.AddDbContext<PropertyDbContext>(options =>
            options.UseNpgsql(connectionString));
      }
      catch (Exception ex)
      {
         Log.Error(ex,"Failed to configure Property Infrastructure: {ErrorMessage}", ex.Message);
         throw;
      }

      services.AddScoped<IPropertyTypeRepository, PropertyTypeRepository>();
      services.AddScoped<IPropertyRepository, PropertyRepository>();
      services.AddScoped<IAmenityRepository, AmenityRepository>();
      services.AddScoped<IPropertyAmenityRepository, PropertyAmenityRepository>();
      services.AddScoped<IUnitOfWork, UnitOfWork>();
      services.AddScoped<PostgresServer>();
   }
}