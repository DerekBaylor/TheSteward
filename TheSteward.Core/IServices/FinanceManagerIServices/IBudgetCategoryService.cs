using TheSteward.Core.Dtos.FinanceManagerDtos;
using TheSteward.Core.Models.FinanceManagerModels;

namespace TheSteward.Core.IServices.FinanceManagerIServices;

public interface IBudgetCategoryService : IBaseService<BudgetCategory>
{

    /// <summary>
    /// Asynchronously creates a new budget category.
    /// </summary>
    /// <param name="categoryDto">The budget category data for creation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the created budget category as <see cref="BudgetCategoryDto"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="categoryDto"/> is null.</exception>
    /// <remarks>
    /// Creates a new budget category associated with the specified budget. 
    /// The category is created without subcategories initially; use the BudgetSubCategoryService to add subcategories.
    /// </remarks>
    /// <example>
    /// <code>
    /// var createDto = new CreateBudgetCategoryDto
    /// {
    ///     BudgetCategoryName = "Housing",
    ///     BudgetId = budgetId,
    ///     DisplayOrder = 1
    /// };
    /// var category = await budgetCategoryService.AddAsync(createDto);
    /// </code>
    /// </example>
    Task<BudgetCategoryDto> AddAsync(CreateBudgetCategoryDto categoryDto);

    /// <summary>
    /// Asynchronously updates an existing budget category.
    /// </summary>
    /// <param name="categoryDto">The budget category data with updated values.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the updated budget category as <see cref="UpdateBudgetCategoryDto"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="categoryDto"/> is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the budget category with the specified ID is not found.</exception>
    /// <remarks>
    /// Updates the category name and display order. Does not affect associated subcategories.
    /// </remarks>
    /// <example>
    /// <code>
    /// var updateDto = new UpdateBudgetCategoryDto
    /// {
    ///     BudgetCategoryId = categoryId,
    ///     BudgetCategoryName = "Housing & Utilities",
    ///     BudgetId = budgetId,
    ///     DisplayOrder = 1
    /// };
    /// var updated = await budgetCategoryService.UpdateAsync(updateDto);
    /// </code>
    /// </example>
    Task<UpdateBudgetCategoryDto> UpdateAsync(UpdateBudgetCategoryDto categoryDto);

    /// <summary>
    /// Asynchronously deletes a budget category by its identifier.
    /// </summary>
    /// <param name="categoryId">The unique identifier of the budget category to delete.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="categoryId"/> is empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the budget category with the specified ID is not found.</exception>
    /// <remarks>
    /// This performs a hard delete. Be aware that this will also delete all associated subcategories 
    /// if your database is configured with cascade delete.
    /// </remarks>
    /// <example>
    /// <code>
    /// await budgetCategoryService.DeleteAsync(categoryId);
    /// </code>
    /// </example>
    Task DeleteAsync(Guid categoryId);

    /// <summary>
    /// Asynchronously retrieves a single budget category by its identifier, including all subcategories.
    /// </summary>
    /// <param name="categoryId">The unique identifier of the budget category to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the budget category as <see cref="BudgetCategoryDto"/>,
    /// or null if not found.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="categoryId"/> is empty.</exception>
    /// <remarks>
    /// This method eagerly loads all associated subcategories and includes them in the returned DTO.
    /// </remarks>
    /// <example>
    /// <code>
    /// var category = await budgetCategoryService.GetAsync(categoryId);
    /// if (category != null)
    /// {
    ///     Console.WriteLine($"Category has {category.BudgetSubCategories.Count} subcategories");
    /// }
    /// </code>
    /// </example>
    Task<BudgetCategoryDto?> GetAsync(Guid categoryId);
    
    /// <summary>
    /// Asynchronously retrieves all budget categories for a specific budget, including subcategories.
    /// </summary>
    /// <param name="budgetId">The unique identifier of the budget.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of all budget categories
    /// associated with the specified budget, ordered by DisplayOrder.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="budgetId"/> is empty.</exception>
    /// <remarks>
    /// Returns an empty list if no categories are found for the specified budget.
    /// Results are ordered by DisplayOrder in ascending order.
    /// Each category includes its associated subcategories.
    /// </remarks>
    /// <example>
    /// <code>
    /// var categories = await budgetCategoryService.GetAllByBudgetIdAsync(budgetId);
    /// foreach (var category in categories)
    /// {
    ///     Console.WriteLine($"{category.BudgetCategoryName}: {category.BudgetSubCategories.Count} subcategories");
    /// }
    /// </code>
    /// </example>
    Task<List<BudgetCategoryDto>> GetAllByBudgetIdAsync(Guid budgetId);






}