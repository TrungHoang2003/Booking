using Property.Domain.Aggregates.RentalUnitAggregate;
using Property.Infrastructure.DbHelper;

namespace Property.Infrastructure.Repositories;

public interface IRentalUnitAmenityRepository : IGenericRepository<RentalUnitAmenity>
{
    
}

public class RentalUnitAmenityRepository(PropertyDbContext dbContext) : GenericRepository<RentalUnitAmenity>(dbContext), IRentalUnitAmenityRepository
{
}