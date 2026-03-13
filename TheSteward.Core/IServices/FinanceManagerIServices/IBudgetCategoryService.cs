using TheSteward.Core.Dtos.FinanceManagerDtos;
using TheSteward.Core.Models.FinanceManagerModels;

namespace TheSteward.Core.IServices.FinanceManagerIServices;

public interface IBudgetCategoryService
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
    Task DeleteAsync(Guid categoryId);

    #region Get Methods

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
    Task<List<BudgetCategoryDto>> GetAllByBudgetIdAsync(Guid budgetId);

    /// <summary>
    /// Retrieves an existing budget category by name within a budget, or creates and persists
    /// a new one if no match is found.
    /// </summary>
    /// <remarks>
    /// The display order of a newly created category is set to the current category count for
    /// that budget, placing it at the end of the list.
    /// </remarks>
    /// <param name="budgetId">The unique identifier of the budget to search within.</param>
    /// <param name="categoryName">
    /// The name of the category to find or create. Must not be null or whitespace.
    /// </param>
    /// <returns>
    /// A <see cref="BudgetCategoryDto"/> for the matched or newly created category.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="budgetId"/> is an empty <see cref="Guid"/>, or
    /// <paramref name="categoryName"/> is null or whitespace.
    /// </exception>
    Task<BudgetCategoryDto> GetByIdOrCreateAsync(Guid budgetId, string categoryName);
    #endregion Get Methods
}