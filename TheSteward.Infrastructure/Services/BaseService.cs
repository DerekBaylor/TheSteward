using TheSteward.Core.IRepositories;
using TheSteward.Core.IServices;

namespace TheSteward.Infrastructure.Services;

public class BaseService<T> : IBaseService<T> where T : class
{
    protected readonly IBaseRepository<T> _repository;

    public BaseService(IBaseRepository<T> baseRepository)
    {
        _repository = baseRepository;
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();

        return entity;
    }

    public virtual async Task UpdateAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        await _repository.UpdateAsync(entity);
        await _repository.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        await _repository.DeleteAsync(entity);
        await _repository.SaveChangesAsync();
    }

    public virtual async Task SoftDeleteAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        await _repository.UpdateAsync(entity);
        await _repository.SaveChangesAsync();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentNullException("ID cannot be empty", "id");

        return await _repository.GetByIdAsync(id);
    }

    public virtual IQueryable<T> GetAll()
    {
        return _repository.GetAll();
    }
    
}
