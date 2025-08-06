using Dapper;
using Property.Infrastructure.DbHelper;

namespace Property.Infrastructure.Repositories;

public interface IPropertyRepository: IGenericRepository<Domain.Aggregates.AggregateRoot.Property>
{
}

public class PropertyRepository(PropertyDbContext dbContext) : GenericRepository<Domain.Aggregates.AggregateRoot.Property>(dbContext), IPropertyRepository
{
}