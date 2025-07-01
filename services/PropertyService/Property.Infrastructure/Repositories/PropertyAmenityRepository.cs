using Property.Domain.Aggregates.AggregateRoot;
using Property.Infrastructure.DbHelper;

namespace Property.Infrastructure.Repositories;

public interface IPropertyAmenityRepository: IGenericRepository<PropertyAmenity>
{
}

public class PropertyAmenityRepository(PropertyDbContext dbContext) : GenericRepository<PropertyAmenity>(dbContext), IPropertyAmenityRepository
{
}