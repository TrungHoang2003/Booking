using BuildingBlocks.Interfaces;
using Property.Infrastructure.DbHelper;
using System.Threading;

namespace Property.Infrastructure.Repositories;

public class UnitOfWork(PropertyDbContext dbContext) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }
}
