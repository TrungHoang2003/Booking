using Dapper;
using Property.Domain.Aggregates.RentalUnitAggregate;
using Property.Infrastructure.DbHelper;

namespace Property.Infrastructure.Repositories;

public interface IRentalUnitRepository : IGenericRepository<RentalUnit>
{
}

public class RentalUnitRepository(PropertyDbContext dbContext, PostgresServer server) : GenericRepository<RentalUnit>(dbContext), IRentalUnitRepository
{
}