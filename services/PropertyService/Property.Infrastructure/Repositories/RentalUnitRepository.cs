using Dapper;
using Property.Domain.Aggregates.RentalUnitAggregate;
using Property.Infrastructure.DbHelper;

namespace Property.Infrastructure.Repositories;

public interface IRentalUnitRepository : IGenericRepository<RentalUnit>
{
    Task<RentalUnit?> GetById(int id);
}

public class RentalUnitRepository(PropertyDbContext dbContext, PostgresServer server) : GenericRepository<RentalUnit>(dbContext), IRentalUnitRepository
{
    public async Task<RentalUnit?> GetById(int id)
    {
        var cnn = server.OpenConnection();
        
        const string sql = "Select * from \"RentalUnits\" where \"Id\" = @id";
        var result = await cnn.QueryFirstOrDefaultAsync<RentalUnit>(sql, new { id });
        return result;
    }
}