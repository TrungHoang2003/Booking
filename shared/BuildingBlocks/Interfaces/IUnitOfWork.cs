namespace BuildingBlocks.Commons;

public interface IUnitOfWork: IDisposable
{
   Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
}