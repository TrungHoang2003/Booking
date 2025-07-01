using Dapper;
using Property.Domain.Aggregates.AmenityAggregate;
using Property.Infrastructure.DbHelper;

namespace Property.Infrastructure.Repositories;

public interface IAmenityRepository : IGenericRepository<Amenity>
{
    Task<Amenity?> GetByIdAsync(int id);
}

public class AmenityRepository(PropertyDbContext dbContext, PostgresServer server) : GenericRepository<Amenity>(dbContext), IAmenityRepository
{
    public async Task<Amenity?> GetByIdAsync(int id)
    {
        var cnn = server.OpenConnection();
        
        const string sql = "Select * from \"Amenities\" where \"Id\" = @id";
        var result = await cnn.QueryFirstOrDefaultAsync<Amenity>(sql);
        return result;
    }
}