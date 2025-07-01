using Dapper;
using Property.Infrastructure.DbHelper;

namespace Property.Infrastructure.Repositories;

public interface IPropertyRepository: IGenericRepository<Domain.Aggregates.AggregateRoot.Property>
{
    Task<Domain.Aggregates.AggregateRoot.Property?> GetByIdAsync(int id);
}

public class PropertyRepository(PropertyDbContext dbContext, PostgresServer server) : GenericRepository<Domain.Aggregates.AggregateRoot.Property>(dbContext), IPropertyRepository
{
    public async Task<Domain.Aggregates.AggregateRoot.Property?> GetByIdAsync(int id)
    {
        var cnn = server.OpenConnection();
        const string sql = "Select * from \"Properties\" where \"Id\" = @id";
        var result = await cnn.QueryFirstOrDefaultAsync<Domain.Aggregates.AggregateRoot.Property>(sql, new { id });
        return result;
    }
}