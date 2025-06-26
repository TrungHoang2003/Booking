using Property.Infrastructure.DbHelper;

namespace Property.Infrastructure.Repositories;

public interface IGenericRepository<T> where T : class
{
    public Task Update(T entity);
    public Task Create(T entity);
    public Task Delete(T entity);
}

public class GenericRepository<T>(PropertyDbContext dbContext):IGenericRepository<T> 
    where T : class
{
    public async Task Create(T entity)
    {
        await dbContext.Set<T>().AddAsync(entity);
    }
    public Task Update(T entity)
    {
        dbContext.Set<T>().Update(entity);
        return Task.CompletedTask;
    }

    public Task Delete(T entity)
    {
        dbContext.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }
}