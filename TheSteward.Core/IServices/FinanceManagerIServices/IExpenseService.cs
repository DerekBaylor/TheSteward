

using TheSteward.Core.Dtos.FinanceManagerDtos;

namespace TheSteward.Core.IServices.FinanceManagerIServices;

public interface IExpenseService
{

    /// <summary>
    /// Asynchronously creates a new expense entry.
    /// </summary>
    /// <param name="expenseDto">The expense data for creation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the created expense as <see cref="ExpenseDto"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="expenseDto"/> is null.</exception>
    /// <remarks>
    /// An expense can optionally be linked to a Credit or Investment account.
    /// Only one link (Credit OR Investment) should be set at a time.
    /// </remarks>
    /// <example>
    /// <code>
    /// var createDto = new CreateExpenseDto
    /// {
    ///     ExpenseName = "Rent",
    ///     DueDay = 1,
    ///     AmountDue = 1500m,
    ///     BudgetId = budgetId,
    ///     BudgetCategoryId = housingCategoryId,
    ///     DisplayOrder = 1
    /// };
    /// var expense = await expenseService.AddAsync(createDto);
    /// </code>
    /// </example>
    Task<ExpenseDto> AddAsync(CreateExpenseDto expenseDto);
    
    /// <summary>
    /// Asynchronously updates an existing expense entry.
    /// </summary>
    /// <param name="expenseDto">The expense data with updated values.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the updated expense as <see cref="UpdateExpenseDto"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="expenseDto"/> is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the expense with the specified ID is not found.</exception>
    /// <remarks>
    /// Updates all expense properties including optional links to Credit or Investment accounts.
    /// </remarks>
    /// <example>
    /// <code>
    /// var updateDto = new UpdateExpenseDto
    /// {
    ///     ExpenseId = expenseId,
    ///     ExpenseName = "Rent - Updated",
    ///     DueDay = 5,
    ///     AmountDue = 1600m,
    ///     BudgetId = budgetId,
    ///     BudgetCategoryId = housingCategoryId,
    ///     DisplayOrder = 1
    /// };
    /// var updated = await expenseService.UpdateAsync(updateDto);
    /// </code>
    /// </example>
    Task<UpdateExpenseDto> UpdateAsync(UpdateExpenseDto expenseDto);
        
    /// <summary>
    /// Asynchronously deletes an expense entry by its identifier.
    /// </summary>
    /// <param name="expenseId">The unique identifier of the expense to delete.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="expenseId"/> is empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the expense with the specified ID is not found.</exception>
    /// <remarks>
    /// This performs a hard delete, permanently removing the expense from the database.
    /// </remarks>
    /// <example>
    /// <code>
    /// await expenseService.DeleteAsync(expenseId);
    /// </code>
    /// </example>
    Task DeleteAsync(Guid expenseId);

    #region Get Methods
    /// <summary>
    /// Asynchronously retrieves a single expense entry by its identifier.
    /// </summary>
    /// <param name="expenseId">The unique identifier of the expense to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the expense as <see cref="ExpenseDto"/>,
    /// or null if not found.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="expenseId"/> is empty.</exception>
    /// <remarks>
    /// This method retrieves only the expense entity without related data (Credit, Investment, Category).
    /// Use <see cref="GetWithRelatedDataAsync"/> to include related entities.
    /// </remarks>
    /// <example>
    /// <code>
    /// var expense = await expenseService.GetAsync(expenseId);
    /// if (expense == null)
    /// {
    ///     Console.WriteLine("Expense not found");
    /// }
    /// </code>
    /// </example>
    Task<ExpenseDto?> GetAsync(Guid expenseId);
    
    /// <summary>
    /// Asynchronously retrieves an expense entry with all related data.
    /// </summary>
    /// <param name="expenseId">The unique identifier of the expense to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the expense with related data as <see cref="ExpenseDto"/>,
    /// or null if not found.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="expenseId"/> is empty.</exception>
    /// <remarks>
    /// This method eagerly loads the BudgetCategory, LinkedCredit, and LinkedInvestment if they exist.
    /// </remarks>
    /// <example>
    /// <code>
    /// var expense = await expenseService.GetWithRelatedDataAsync(expenseId);
    /// if (expense?.LinkedCredit != null)
    /// {
    ///     Console.WriteLine($"Linked to credit: {expense.LinkedCredit.CreditName}");
    /// }
    /// if (expense?.BudgetCategory != null)
    /// {
    ///     Console.WriteLine($"Category: {expense.BudgetCategory.BudgetCategoryName}");
    /// }
    /// </code>
    /// </example>
    Task<ExpenseDto?> GetWithRelatedDataAsync(Guid expenseId);
    
    /// <summary>
    /// Asynchronously retrieves all expense entries for a specific budget.
    /// </summary>
    /// <param name="budgetId">The unique identifier of the budget.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of all expenses
    /// associated with the specified budget, ordered by DisplayOrder.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="budgetId"/> is empty.</exception>
    /// <remarks>
    /// Returns an empty list if no expenses are found for the specified budget.
    /// Results are ordered by DisplayOrder in ascending order.
    /// This method does not include related data. Use <see cref="GetAllByBudgetIdWithRelatedDataAsync"/> for that.
    /// </remarks>
    /// <example>
    /// <code>
    /// var expenses = await expenseService.GetAllByBudgetIdAsync(budgetId);
    /// var totalMonthlyExpenses = expenses.Sum(e => e.AmountDue);
    /// </code>
    /// </example>
    Task<List<ExpenseDto>> GetAllByBudgetIdAsync(Guid budgetId);
    
    /// <summary>
    /// Asynchronously retrieves all expense entries for a specific budget category.
    /// </summary>
    /// <param name="categoryId">The unique identifier of the budget category.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of all expenses
    /// associated with the specified category, ordered by DisplayOrder.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="categoryId"/> is empty.</exception>
    /// <remarks>
    /// Returns an empty list if no expenses are found for the specified category.
    /// Results are ordered by DisplayOrder in ascending order.
    /// </remarks>
    /// <example>
    /// <code>
    /// var housingExpenses = await expenseService.GetAllByCategoryIdAsync(housingCategoryId);
    /// var totalHousingCost = housingExpenses.Sum(e => e.AmountDue);
    /// </code>
    /// </example>
    Task<List<ExpenseDto>> GetAllByCategoryIdAsync(Guid categoryId);
    /// <summary>
    /// Asynchronously retrieves all expense entries for a specific budget with all related data.
    /// </summary>
    /// <param name="budgetId">The unique identifier of the budget.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of all expenses
    /// with their related data, ordered by DisplayOrder.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="budgetId"/> is empty.</exception>
    /// <remarks>
    /// This method eagerly loads BudgetCategory, LinkedCredit, and LinkedInvestment for all expenses.
    /// Use this when you need complete expense information with related entities.
    /// </remarks>
    /// <example>
    /// <code>
    /// var expenses = await expenseService.GetAllByBudgetIdWithRelatedDataAsync(budgetId);
    /// foreach (var expense in expenses)
    /// {
    ///     Console.WriteLine($"{expense.ExpenseName} - {expense.BudgetCategory?.BudgetCategoryName}");
    ///     if (expense.LinkedCredit != null)
    ///     {
    ///         Console.WriteLine($"  Linked to credit: {expense.LinkedCredit.CreditName}");
    ///     }
    /// }
    /// </code>
    /// </example>
    Task<List<ExpenseDto>> GetAllByBudgetIdWithRelatedDataAsync(Guid budgetId);

    /// <summary>
    /// Retrieves a single expense by budget ID and expense name.
    /// </summary>
    /// <param name="budgetId">The unique identifier of the budget to search within.</param>
    /// <param name="expenseName">The name of the expense to retrieve.</param>
    /// <returns>
    /// A <see cref="ExpenseDto"/> representing the matching expense.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="budgetId"/> is an empty <see cref="Guid"/>,
    /// or when <paramref name="expenseName"/> is null or whitespace.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// Thrown when no expense with the given <paramref name="expenseName"/> exists
    /// within the budget identified by <paramref name="budgetId"/>.
    /// </exception>
    Task<ExpenseDto> GetByBudgetIdAndExpenseNameAsync(Guid budgetId, string expenseNAme);
    #endregion
}