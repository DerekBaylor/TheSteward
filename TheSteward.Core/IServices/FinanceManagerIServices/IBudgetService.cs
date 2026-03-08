using TheSteward.Core.Dtos.FinanceManagerDtos;
using TheSteward.Core.Dtos.HouseholdDtos;
using TheSteward.Core.Models.FinanceManagerModels;

namespace TheSteward.Core.IServices.FinanceManagerIServices;

public interface IBudgetService
{
    /// <summary>
    /// Asynchronously creates a new budget.
    /// </summary>
    /// <param name="budgetDto">The budget data for creation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the created budget as <see cref="BudgetDto"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="budgetDto"/> is null.</exception>
    /// <remarks>
    /// Creates an empty budget. Use <see cref="AddStarterBudget"/> to create a budget with default categories and sample data.
    /// </remarks>
    /// <example>
    /// <code>
    /// var createDto = new CreateBudgetDto
    /// {
    ///     BudgetName = "My 2024 Budget",
    ///     OwnerId = userId,
    ///     HouseholdId = householdId,
    ///     IsDefaultBudget = true,
    ///     IsPublic = false
    /// };
    /// var budget = await budgetService.AddAsync(createDto);
    /// </code>
    /// </example>
    Task<BudgetDto> AddAsync(CreateBudgetDto budgetDto);

    /// <summary>
    /// Asynchronously creates a starter budget with predefined categories, subcategories, and sample data.
    /// </summary>
    /// <param name="userHouseholdDto">
    /// The user household context used to associate the budget with the correct household and owner.
    /// <see cref="UserHouseholdDto.HouseholdId"/> is used as the budget's household FK,
    /// and <see cref="UserHouseholdDto.UserId"/> is used as the budget's owner.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the created starter budget as <see cref="BudgetDto"/>
    /// with all related categories, subcategories, expenses, income, credits, and investments loaded.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <see cref="UserHouseholdDto.UserHouseholdId"/> is empty.</exception>
    /// <exception cref="KeyNotFoundException">
    /// Thrown when expected seed expenses ("Credit Card Payment" or "Savings Account") are not found during creation.
    /// </exception>
    /// <remarks>
    /// This method runs inside a database transaction. If any step fails, all changes are rolled back
    /// and no partial data is left in the database.
    /// This method creates a comprehensive starter budget including:
    /// <list type="bullet">
    /// <item><description>Standard budget categories (Housing, Transportation, Living Expenses, etc.)</description></item>
    /// <item><description>Common subcategories for each category</description></item>
    /// <item><description>Sample expenses for common bills</description></item>
    /// <item><description>A sample income entry</description></item>
    /// <item><description>A sample credit card linked to its expense</description></item>
    /// <item><description>A sample savings investment linked to its expense</description></item>
    /// </list>
    /// All monetary values are placeholders that the user should update.
    /// </remarks>
    Task<BudgetDto> AddStarterBudget(UserHouseholdDto userHouseholdDto);
    
    /// <summary>
    /// Asynchronously deletes a budget by its identifier.
    /// </summary>
    /// <param name="budgetId">The unique identifier of the budget to delete.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="budgetId"/> is empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the budget with the specified ID is not found.</exception>
    /// <remarks>
    /// This performs a hard delete. All related data (categories, expenses, incomes, etc.) will be deleted
    /// if cascade delete is configured in your database.
    /// </remarks>
    /// <example>
    /// <code>
    /// await budgetService.DeleteAsync(budgetId);
    /// </code>
    /// </example>
    Task DeleteAsync(Guid budgetId);

    /// <summary>
    /// Asynchronously updates an existing budget.
    /// </summary>
    /// <param name="budgetDto">The budget data with updated values.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the updated budget as <see cref="BudgetDto"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="budgetDto"/> is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the budget with the specified ID is not found.</exception>
    /// <example>
    /// <code>
    /// var updateDto = new UpdateBudgetDto
    /// {
    ///     BudgetId = budgetId,
    ///     BudgetName = "Updated Budget Name",
    ///     IsDefaultBudget = true,
    ///     IsPublic = false
    /// };
    /// var updated = await budgetService.UpdateAsync(updateDto);
    /// </code>
    /// </example>
    Task<BudgetDto> UpdateAsync(UpdateBudgetDto budgetDto);

    #region Get Methods
    /// <summary>
    /// Asynchronously retrieves a budget by its identifier with all related data.
    /// </summary>
    /// <param name="budgetId">The unique identifier of the budget to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the budget with all related data as <see cref="BudgetDto"/>.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="budgetId"/> is empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the budget with the specified ID is not found.</exception>
    /// <remarks>
    /// This method eagerly loads all related entities: Categories (with subcategories), Incomes, Expenses, Credits, and Investments.
    /// </remarks>
    /// <example>
    /// <code>
    /// var budget = await budgetService.GetByIdAsync(budgetId);
    /// Console.WriteLine($"Budget has {budget.Incomes.Count} income sources");
    /// </code>
    /// </example>
    Task<BudgetDto> GetByIdAsync(Guid budgetId);
    
    /// <summary>
    /// Asynchronously retrieves all budgets for a specific household.
    /// </summary>
    /// <param name="userHouseholdId">The unique identifier of the household.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of all budgets for the household.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="userHouseholdId"/> is empty.</exception>
    /// <remarks>
    /// Returns budgets ordered by: default budgets first, then by creation date descending.
    /// This method loads basic budget information without related entities for performance.
    /// </remarks>
    /// <example>
    /// <code>
    /// var budgets = await budgetService.GetBudgetsByUserHouseholdIdAsync(householdId);
    /// var defaultBudget = budgets.FirstOrDefault(b => b.IsDefaultBudget);
    /// </code>
    /// </example>
    Task<List<BudgetDto>> GetBudgetsByUserHouseholdIdAsync(Guid  userHouseholdId);
    
    /// <summary>
    /// Asynchronously retrieves the default budget for a household with all related data.
    /// </summary>
    /// <param name="householdId">The unique identifier of the household.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the default budget with all related data.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="householdId"/> is empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when no default budget exists for the household.</exception>
    /// <remarks>
    /// This method retrieves the budget marked as default for the household, including all related entities.
    /// </remarks>
    /// <example>
    /// <code>
    /// var defaultBudget = await budgetService.GetByHouseholdAsync(householdId);
    /// </code>
    /// </example>
    Task<List<BudgetDto>> GetByHouseholdAsync(Guid householdId);

    /// <summary>
    /// Asynchronously retrieves all budgets the user is permitted to view within their household context.
    /// </summary>
    /// <param name="userHouseholdDto">
    /// The user household context used to determine which budgets are accessible.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of accessible
    /// <see cref="BudgetDto"/> objects with all related entities loaded, ordered by:
    /// the user's personal default budget first, then household default budgets, then by creation date descending.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <see cref="UserHouseholdDto.UserHouseholdId"/> is empty.</exception>
    /// <remarks>
    /// A budget is accessible to the user if either of the following conditions is true:
    /// <list type="bullet">
    /// <item><description>The budget's <c>OwnerId</c> matches the user's <c>UserId</c>.</description></item>
    /// <item><description>The budget belongs to the user's household and <c>IsPublic</c> is <c>true</c>.</description></item>
    /// </list>
    /// This method eagerly loads all related entities: Categories (with subcategories), Incomes, Expenses, Credits, and Investments.
    /// </remarks>
    /// <example>
    /// <code>
    /// var budgets = await budgetService.GetBudgetsForUserHouseholdAsync(userHouseholdDto);
    /// var defaultBudget = budgets.FirstOrDefault();
    /// </code>
    /// </example>
    Task<List<BudgetDto>> GetBudgetsForUserHouseholdAsync(UserHouseholdDto userHouseholdDto);
    #endregion Get Methods
}