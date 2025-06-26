using Property.Domain.ValueObjects;
using Property.Infrastructure.DbHelper;

namespace Property.Infrastructure.Repositories;

public interface IBedTypeRepository : IGenericRepository<BedType>
{
}

public class BedTypeRepository(PropertyDbContext dbContext) : GenericRepository<BedType>(dbContext), IBedTypeRepository
{
    
}