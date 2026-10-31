namespace TicketFlow.Data;

public interface IBaseRepository
{
    Task AddAsync<T>(T entity) where T : class;
    Task AddRangeAsync<T>(IEnumerable<T> entities) where T : class;
    Task SaveChangesAsync();

    void Update<T>(T entity) where T : class;
    void Remove<T>(T entity) where T : class;
    void RemoveRange<T>(IEnumerable<T> entities) where T : class;

    IQueryable<T> GetQueryable<T>() where T : class;
    IQueryable<T> GetReadOnlyQueryable<T>() where T : class;
}