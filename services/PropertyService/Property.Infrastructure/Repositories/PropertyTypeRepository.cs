using Dapper;
using Property.Domain.Aggregates.AggregateRoot;
using Property.Infrastructure.DbHelper;

namespace Property.Infrastructure.Repositories;

public interface IPropertyTypeRepository : IGenericRepository<PropertyType>
{
    public Task<List<PropertyType>> GetRoomBasedPropertyTypes();
    public Task<List<PropertyType>> GetEntirePropertyTypes();
    public Task<PropertyType?> GetById(int id);
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

    public async Task<PropertyType?> GetById(int id)
    {
        var cnn = server.OpenConnection();
        try
        {
            const string sql = "SELECT * FROM \"PropertyTypes\" WHERE \"Id\" = @id";
            var result = await cnn.QueryFirstOrDefaultAsync<PropertyType>(sql, new { id });
            return result;
        }
        catch (Exception e)
        {
            throw new Exception($"Error while getting property type by id {id}", e);
        }
    }
}