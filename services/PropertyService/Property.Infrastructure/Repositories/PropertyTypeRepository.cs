using Dapper;
using Property.Domain.Aggregates.AggregateRoot;
using Property.Infrastructure.DbHelper;

namespace Property.Infrastructure.Repositories;

public interface IPropertyTypeRepository : IGenericRepository<PropertyType>
{
    public Task<List<PropertyType>> GetRoomBasedPropertyTypes();
    public Task<List<PropertyType>> GetEntirePropertyTypes();
}
public class PropertyTypeRepository(PropertyDbContext dbContext, PostgresServer server) :GenericRepository<PropertyType>(dbContext), IPropertyTypeRepository
{
    public async Task<List<PropertyType>> GetRoomBasedPropertyTypes()
    {
        var cnn = server.OpenConnection();
        try
        {
            const string sql = "select * from \"PropertyTypes\" where \"IsRoomBased\" = true";
            var result = await cnn.QueryAsync<PropertyType>(sql);
            return result.ToList();
        }
        catch (Exception e)
        {
            throw new Exception("Error while getting room-based property types", e);
        }
    }

    public async Task<List<PropertyType>> GetEntirePropertyTypes()
    {
        var cnn = server.OpenConnection();
        try
        {
            const string sql = "SELECT * FROM \"PropertyTypes\" WHERE \"IsRoomBased\" = false";
            var result = await cnn.QueryAsync<PropertyType>(sql);
            return result.ToList();
        }
        catch (Exception e)
        {
            throw new Exception("Error while getting entire property types", e);
        }
    }
}