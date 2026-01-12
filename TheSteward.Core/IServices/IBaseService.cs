namespace TheSteward.Core.IServices;

public interface IBaseService<T> where T : class
{
    /// <summary>
    /// Asynchronously adds a new entity to the database.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the added entity,
    /// which may include database-generated values such as IDs or timestamps.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is null.</exception>
    /// <remarks>
    /// This method persists changes to the database immediately after adding the entity.
    /// Override this method in derived classes to add custom validation or business logic
    /// before or after the entity is added.
    /// </remarks>
    /// <example>
    /// <code>
    /// var newBudget = new Budget { Name = "Monthly Budget", OwnerId = "user123" };
    /// var addedBudget = await budgetService.AddAsync(newBudget);
    /// Console.WriteLine($"Created budget with ID: {addedBudget.BudgetId}");
    /// </code>
    /// </example>
    Task<T> AddAsync(T entity);
    
    /// <summary>
    /// Asynchronously updates an existing entity in the database.
    /// </summary>
    /// <param name="entity">The entity with updated values to persist.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is null.</exception>
    /// <remarks>
    /// This method assumes the entity is already being tracked by the context or will be attached
    /// by the repository. Changes are persisted to the database immediately.
    /// Override this method in derived classes to add validation, audit logging, or other
    /// business logic before or after the update.
    /// </remarks>
    /// <example>
    /// <code>
    /// var budget = await budgetService.GetByIdAsync(budgetId);
    /// budget.Name = "Updated Budget Name";
    /// await budgetService.UpdateAsync(budget);
    /// </code>
    /// </example>
    Task UpdateAsync(T entity);
    
    /// <summary>
    /// Asynchronously performs a hard delete of an entity from the database.
    /// </summary>
    /// <param name="entity">The entity to permanently remove from the database.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is null.</exception>
    /// <remarks>
    /// This method permanently removes the entity from the database. The deletion is immediate
    /// and cannot be undone. For entities that should support recovery or audit trails,
    /// consider using <see cref="SoftDeleteAsync"/> instead.
    /// Override this method in derived classes to handle related entity cleanup or to enforce
    /// business rules before deletion.
    /// </remarks>
    /// <example>
    /// <code>
    /// var budget = await budgetService.GetByIdAsync(budgetId);
    /// if (budget != null)
    /// {
    ///     await budgetService.DeleteAsync(budget);
    /// }
    /// </code>
    /// </example>
    Task DeleteAsync(T entity);
    
    /// <summary>
    /// Asynchronously performs a soft delete of an entity, marking it as deleted without removing it from the database.
    /// </summary>
    /// <param name="entity">The entity to mark as deleted. The entity should have a property indicating deletion status (e.g., IsDeleted, DeletedDate).</param>
    /// <returns>A task that represents the asynchronous soft delete operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is null.</exception>
    /// <remarks>
    /// This method marks an entity as deleted by updating its deletion flag or timestamp,
    /// preserving the data in the database for audit trails or potential recovery.
    /// The specific implementation depends on the entity having appropriate soft delete properties.
    /// Derived classes should override this method to set the appropriate soft delete properties
    /// on the entity before calling this base implementation.
    /// </remarks>
    /// <example>
    /// <code>
    /// // In a derived BudgetService class:
    /// public override async Task SoftDeleteAsync(Budget entity)
    /// {
    ///     entity.IsDeleted = true;
    ///     entity.DeletedDate = DateTime.UtcNow;
    ///     await base.SoftDeleteAsync(entity);
    /// }
    /// </code>
    /// </example>
    Task SoftDeleteAsync(T entity);
    
    /// <summary>
    /// Asynchronously retrieves an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the entity
    /// if found; otherwise, null.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="id"/> is <see cref="Guid.Empty"/>.</exception>
    /// <remarks>
    /// This method retrieves only the entity itself without any related entities.
    /// Override this method in derived classes to include related entities using eager loading
    /// or to apply additional filtering (e.g., excluding soft-deleted records).
    /// </remarks>
    /// <example>
    /// <code>
    /// var budget = await budgetService.GetByIdAsync(budgetId);
    /// if (budget == null)
    /// {
    ///     return NotFound($"Budget with ID {budgetId} not found");
    /// }
    /// return Ok(budget);
    /// </code>
    /// </example>
    Task<T?> GetByIdAsync(Guid id);
    
    /// <summary>
    /// Retrieves all entities as a queryable collection.
    /// </summary>
    /// <returns>
    /// An <see cref="IQueryable{T}"/> that can be used to further filter, sort, or project entities
    /// before executing the query against the database.
    /// </returns>
    /// <remarks>
    /// This method returns an <see cref="IQueryable{T}"/> rather than executing the query immediately,
    /// allowing for deferred execution and query composition. Additional LINQ operations (such as
    /// Where, OrderBy, Select, Include) can be chained before the query is materialized.
    /// 
    /// <para>
    /// <strong>Important:</strong> The query is not executed until the result is enumerated
    /// (e.g., by calling ToList(), ToArray(), FirstOrDefault(), or iterating with foreach).
    /// This enables building complex queries efficiently without multiple database roundtrips.
    /// </para>
    /// 
    /// <para>
    /// Override this method in derived classes to apply default filtering (e.g., excluding
    /// soft-deleted records), sorting, or to include related entities by default.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Basic usage - retrieve all entities
    /// var allBudgets = await budgetService.GetAll().ToListAsync();
    /// 
    /// // Chained with additional filtering
    /// var publicBudgets = await budgetService.GetAll()
    ///     .Where(b => b.IsPublic)
    ///     .OrderBy(b => b.CreatedDate)
    ///     .ToListAsync();
    /// 
    /// // With projections to DTOs
    /// var budgetSummaries = await budgetService.GetAll()
    ///     .Select(b => new BudgetSummaryDto 
    ///     { 
    ///         BudgetId = b.BudgetId, 
    ///         Name = b.Name 
    ///     })
    ///     .ToListAsync();
    /// 
    /// // Include related entities
    /// var budgetsWithIncomes = await budgetService.GetAll()
    ///     .Include(b => b.Incomes)
    ///     .ToListAsync();
    /// </code>
    /// </example>
    IQueryable<T> GetAll();
}