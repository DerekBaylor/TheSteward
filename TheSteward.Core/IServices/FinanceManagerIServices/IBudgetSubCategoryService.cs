using TheSteward.Core.Dtos.FinanceManagerDtos;
using TheSteward.Core.Models.FinanceManagerModels;

namespace TheSteward.Core.IServices.FinanceManagerIServices;

public interface IBudgetSubCategoryService
{

    /// <summary>
    /// Asynchronously creates a new budget subcategory.
    /// </summary>
    /// <param name="subCategoryDto">The budget subcategory data for creation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the created budget subcategory as <see cref="BudgetSubCategoryDto"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="subCategoryDto"/> is null.</exception>
    /// <remarks>
    /// Creates a new subcategory under an existing budget category.
    /// Both BudgetId and BudgetCategoryId must reference existing entities.
    /// </remarks>
    Task<BudgetSubCategoryDto> AddAsync(CreateBudgetSubCategoryDto subCategoryDto);

    /// <summary>
    /// Asynchronously updates an existing budget subcategory.
    /// </summary>
    /// <param name="subCategoryDto">The budget subcategory data with updated values.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the updated budget subcategory as <see cref="UpdateBudgetSubCategoryDto"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="subCategoryDto"/> is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the budget subcategory with the specified ID is not found.</exception>
    /// <remarks>
    /// Updates the subcategory name, display order, and parent category association.
    /// </remarks>
    Task<BudgetSubCategoryDto> UpdateAsync(UpdateBudgetSubCategoryDto subCategoryDto);

    /// <summary>
    /// Asynchronously deletes a budget subcategory by its identifier.
    /// </summary>
    /// <param name="subCategoryId">The unique identifier of the budget subcategory to delete.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="subCategoryId"/> is empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the budget subcategory with the specified ID is not found.</exception>
    /// <remarks>
    /// This performs a hard delete, permanently removing the subcategory from the database.
    /// </remarks>
    Task DeleteAsync(Guid subCategoryId);

    #region Get Methods
    /// <summary>
    /// Asynchronously retrieves a single budget subcategory by its identifier.
    /// </summary>
    /// <param name="subCategoryId">The unique identifier of the budget subcategory to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the budget subcategory as <see cref="BudgetSubCategoryDto"/>,
    /// or null if not found.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="subCategoryId"/> is empty.</exception>
    Task<BudgetSubCategoryDto?> GetAsync(Guid subCategoryId);

    /// <summary>
    /// Asynchronously retrieves all budget subcategories for a specific budget category.
    /// </summary>
    /// <param name="categoryId">The unique identifier of the parent budget category.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of all budget subcategories
    /// associated with the specified category, ordered by DisplayOrder.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="categoryId"/> is empty.</exception>
    /// <remarks>
    /// Returns an empty list if no subcategories are found for the specified category.
    /// Results are ordered by DisplayOrder in ascending order.
    /// </remarks>
    Task<List<BudgetSubCategoryDto>> GetAllByCategoryIdAsync(Guid categoryId);

    /// <summary>
    /// Asynchronously retrieves all budget subcategories for a specific budget.
    /// </summary>
    /// <param name="budgetId">The unique identifier of the budget.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of all budget subcategories
    /// associated with the specified budget, ordered by DisplayOrder.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="budgetId"/> is empty.</exception>
    /// <remarks>
    /// Returns an empty list if no subcategories are found for the specified budget.
    /// This returns all subcategories across all categories within the budget.
    /// Results are ordered by DisplayOrder in ascending order.
    /// </remarks>
    Task<List<BudgetSubCategoryDto>> GetAllByBudgetIdAsync(Guid budgetId);

    /// <summary>
    /// Retrieves an existing subcategory by name within a budget category, or creates and
    /// persists a new one if no match is found.
    /// </summary>
    /// <remarks>
    /// Matching is performed against both <paramref name="budgetId"/> and
    /// <paramref name="budgetCategoryId"/> to prevent collisions across categories that share
    /// a subcategory name. The display order of a newly created subcategory is set to the
    /// current subcategory count for that parent category, placing it at the end of the list.
    /// </remarks>
    /// <param name="budgetId">The unique identifier of the budget.</param>
    /// <param name="budgetCategoryId">The unique identifier of the parent category.</param>
    /// <param name="subCategoryName">
    /// The name of the subcategory to find or create. Must not be null or whitespace.
    /// </param>
    /// <returns>
    /// A <see cref="BudgetSubCategoryDto"/> for the matched or newly created subcategory.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="budgetId"/> or <paramref name="budgetCategoryId"/> is an
    /// empty <see cref="Guid"/>, or <paramref name="subCategoryName"/> is null or whitespace.
    /// </exception>
    Task<BudgetSubCategoryDto> GetByIdOrCreateSubCategoryAsync(Guid budgetId, Guid budgetCategoryId, string subCategoryName);

    #endregion GetMethods
}