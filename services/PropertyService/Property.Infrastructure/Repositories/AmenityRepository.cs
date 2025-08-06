using Dapper;
using Property.Domain.Aggregates.AmenityAggregate;
using Property.Infrastructure.DbHelper;

namespace Property.Infrastructure.Repositories;

public interface IAmenityRepository : IGenericRepository<Amenity>
{
}

public class AmenityRepository(PropertyDbContext dbContext, PostgresServer server) : GenericRepository<Amenity>(dbContext), IAmenityRepository
{
}