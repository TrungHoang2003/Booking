using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Property.Infrastructure.DbHelper;
using Serilog;

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
   }
}