using Property.Domain.Aggregates.BedroomAggregate;
using Property.Infrastructure.DbHelper;

namespace Property.Infrastructure.Repositories;

public interface IBedroomRepository : IGenericRepository<Bedroom>;

public class BedroomRepository(PropertyDbContext dbContext) : GenericRepository<Bedroom>(dbContext), IGenericRepository<Bedroom>
{
    
}