using Microsoft.EntityFrameworkCore.Storage;

namespace TheSteward.Core.IRepositories;

public interface IBaseRepository<T> where T : class
{
    /// <summary>
    /// Retrieves a single entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. 
    /// The task result contains the entity if found; otherwise, null.
    /// </returns>
    Task<T?> GetByIdAsync(Guid id);

    /// <summary>
    /// Returns an IQueryable of all entities, allowing for deferred execution and additional filtering.
    /// </summary>
    /// <returns>
    /// An IQueryable&lt;T&gt; that can be further filtered, sorted, or projected before execution.
    /// </returns>
    /// <remarks>
    /// This method does not execute the query immediately. Use .ToListAsync(), .FirstOrDefaultAsync(), 
    /// or similar methods to execute the query. This approach is more efficient as it allows 
    /// filtering to occur at the database level.
    /// </remarks>
    IQueryable<T> GetAll();

    /// <summary>
    /// Retrieves all entities from the database as a materialized collection.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. 
    /// The task result contains an IEnumerable&lt;T&gt; with all entities.
    /// </returns>
    /// <remarks>
    /// This method executes the query immediately and loads all entities into memory.
    /// Use GetAll() instead if you need to apply filters before execution for better performance.
    /// </remarks>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Adds a new entity to the database context.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <remarks>
    /// This method only adds the entity to the context. 
    /// You must call SaveChangesAsync() to persist the changes to the database.
    /// </remarks>
    Task AddAsync(T entity);

    /// <summary>
    /// Marks an existing entity as modified in the database context.
    /// </summary>
    /// <param name="entity">The entity with updated values.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <remarks>
    /// This method marks the entity as modified in the context. 
    /// You must call SaveChangesAsync() to persist the changes to the database.
    /// The entity must already exist in the database.
    /// </remarks>
    Task UpdateAsync(T entity);

    /// <summary>
    /// Marks an entity for deletion from the database context.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <remarks>
    /// This method marks the entity for deletion in the context. 
    /// You must call SaveChangesAsync() to persist the deletion to the database.
    /// </remarks>
    Task DeleteAsync(T entity);

    /// <summary>
    /// Saves all pending changes in the database context to the database.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. 
    /// The task result contains the number of state entries written to the database.
    /// </returns>
    /// <remarks>
    /// This method commits all changes made through AddAsync, UpdateAsync, and DeleteAsync 
    /// to the database in a single transaction. If any operation fails, all changes are rolled back.
    /// </remarks>
    Task<int> SaveChangesAsync();

    /// <summary>
    /// Begins a new database transaction on the underlying <see cref="TheStewardContext"/>.
    /// Because all repositories share the same scoped <see cref="TheStewardContext"/> instance,
    /// a transaction started here will encompass all subsequent operations across any repository
    /// until it is either committed or rolled back.
    /// </summary>
    /// <returns>
    /// A <see cref="IDbContextTransaction"/> representing the active transaction.
    /// The caller is responsible for calling <see cref="IDbContextTransaction.CommitAsync"/> 
    /// on success or <see cref="IDbContextTransaction.RollbackAsync"/> on failure.
    /// </returns>
    /// <remarks>
    /// It is recommended to use this method within a try/catch block and to dispose the transaction
    /// with <see langword="await using"/> to ensure proper cleanup regardless of outcome:
    /// <code>
    /// await using var transaction = await _budgetRepository.BeginTransactionAsync();
    /// try
    /// {
    ///     // perform operations
    ///     await transaction.CommitAsync();
    /// }
    /// catch
    /// {
    ///     await transaction.RollbackAsync();
    ///     throw;
    /// }
    /// </code>
    /// </remarks>
    Task<IDbContextTransaction> BeginTransactionAsync();

    /// <summary>
    /// Clears all tracked changes, resetting the change tracker to its initial state.
    /// </summary>
    /// <remarks>Call this method to discard any pending changes that have been tracked. After calling this
    /// method, the change tracker will not report any changes until new modifications are made.</remarks>
    void ClearChangeTracker();
}