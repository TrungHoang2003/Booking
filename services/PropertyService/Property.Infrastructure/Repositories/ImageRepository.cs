using Property.Domain.Aggregates.ImageAggregate;
using Property.Infrastructure.DbHelper;

namespace Property.Infrastructure.Repositories;

public interface IImageRepository : IGenericRepository<Image>
{
    
}

public class ImageRepository(PropertyDbContext dbContext) : GenericRepository<Image>(dbContext), IImageRepository
{
    
}