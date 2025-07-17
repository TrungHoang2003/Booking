using BuildingBlocks.Interfaces;
using Identity.Infrastructure.DbHelper;

namespace Identity.Infrastructure.Repositories;

public class UnitOfWork(IdentityDbContext dbContext): IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }
}
