using BuildingBlocks.Interfaces;
using Microsoft.EntityFrameworkCore;
using Property.Infrastructure.DbHelper;

namespace Property.Infrastructure.Repositories;

public interface IGenericRepository<T> where T : class, IEntity
{
    public Task<T> GetById(int id);
    public Task Update(T entity);
    public Task Create(T entity);
    public Task Delete(T entity);
    public Task AddRangeAsync(List<T> entities);
}

public class GenericRepository<T>(PropertyDbContext dbContext):IGenericRepository<T> 
    where T : class, IEntity
{
    public async Task AddRangeAsync(List<T> entities)
    {
        await dbContext.Set<T>().AddRangeAsync(entities);
    }
    public async Task Create(T entity)
    {
        await dbContext.Set<T>().AddAsync(entity);
    }

    public async Task<T> GetById(int id)
    {
        return await dbContext.Set<T>().FirstOrDefaultAsync(e => e.Id == id)
               ?? throw new Exception($"{typeof(T).Name} with Id = {id} not found");
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