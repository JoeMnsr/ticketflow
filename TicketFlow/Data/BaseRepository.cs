using Microsoft.EntityFrameworkCore;

namespace TicketFlow.Data;

public class BaseRepository(AppDbContext context) : IBaseRepository
{
    public async Task AddAsync<T>(T entity) where T : class
    {
        await context.Set<T>().AddAsync(entity);
    }

    public async Task AddRangeAsync<T>(IEnumerable<T> entities) where T : class
    {
        await context.Set<T>().AddRangeAsync(entities);
    }

    public void Update<T>(T entity) where T : class
    {
        context.Set<T>().Update(entity);
    }

    public void Remove<T>(T entity) where T : class
    {
        context.Set<T>().Remove(entity);
    }

    public void RemoveRange<T>(IEnumerable<T> entities) where T : class
    {
        context.Set<T>().RemoveRange(entities);
    }

    public IQueryable<T> GetQueryable<T>() where T : class
    {
        return context.Set<T>().AsQueryable();
    }

    public IQueryable<T> GetReadOnlyQueryable<T>() where T : class
    {
        return context.Set<T>().AsNoTracking();
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}