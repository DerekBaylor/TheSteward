namespace TheSteward.Core.IServices;

public interface IBaseService<T> where T : class
{
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task SoftDeleteAsync(T entity);
    Task<T?> GetByIdAsync(Guid id);
}