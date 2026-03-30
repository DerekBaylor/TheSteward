using TheSteward.Core.Dtos.FinanceManagerDtos;

namespace TheSteward.Core.IServices.FinanceManagerIServices;

public interface IInvestmentService
{
    /// <summary>
    /// Asynchronously creates a new investment entry.
    /// </summary>
    /// <param name="investmentDto">The investment data for creation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the created investment as <see cref="InvestmentDto"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="investmentDto"/> is null.</exception>
    /// <remarks>
    /// The EstYearlyGrowth should be calculated on the frontend before calling this method.
    /// An investment can optionally be linked to an Expense for automatic contribution tracking.
    /// </remarks>
    /// <example>
    /// <code>
    /// var createDto = new CreateInvestmentDto
    /// {
    ///     InvestmentName = "401(k)",
    ///     CurrentValue = 50000m,
    ///     InterestRate = 0.07m, // 7%
    ///     ContributionAmount = 500m,
    ///     ContributionFrequency = 26, // Bi-weekly
    ///     EstYearlyGrowth = 4200m, // Calculated on frontend
    ///     BudgetId = budgetId,
    ///     DisplayOrder = 1
    /// };
    /// var investment = await investmentService.AddAsync(createDto);
    /// </code>
    /// </example>
    Task<InvestmentDto> AddAsync(CreateInvestmentDto investmentDto);
    
    /// <summary>
    /// Asynchronously updates an existing investment entry.
    /// </summary>
    /// <param name="investmentDto">The investment data with updated values.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the updated investment as <see cref="UpdateInvestmentDto"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="investmentDto"/> is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the investment with the specified ID is not found.</exception>
    /// <remarks>
    /// The EstYearlyGrowth should be recalculated on the frontend before calling this method.
    /// </remarks>
    /// <example>
    /// <code>
    /// var updateDto = new UpdateInvestmentDto
    /// {
    ///     InvestmentId = investmentId,
    ///     InvestmentName = "401(k) - Updated",
    ///     CurrentValue = 55000m, // Grown
    ///     InterestRate = 0.07m,
    ///     ContributionAmount = 600m, // Increased contribution
    ///     ContributionFrequency = 26,
    ///     EstYearlyGrowth = 4800m, // Recalculated
    ///     BudgetId = budgetId,
    ///     DisplayOrder = 1
    /// };
    /// var updated = await investmentService.UpdateAsync(updateDto);
    /// </code>
    /// </example>
    Task<InvestmentDto> UpdateAsync(UpdateInvestmentDto investmentDto);
    
    /// <summary>
    /// Asynchronously deletes an investment entry by its identifier.
    /// </summary>
    /// <param name="investmentId">The unique identifier of the investment to delete.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="investmentId"/> is empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the investment with the specified ID is not found.</exception>
    /// <remarks>
    /// This performs a hard delete, permanently removing the investment from the database.
    /// Consider the impact on linked expenses before deleting.
    /// </remarks>
    /// <example>
    /// <code>
    /// await investmentService.DeleteAsync(investmentId);
    /// </code>
    /// </example>
    Task DeleteAsync(Guid investmentId);
    
    /// <summary>
    /// Asynchronously retrieves a single investment entry by its identifier.
    /// </summary>
    /// <param name="investmentId">The unique identifier of the investment to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the investment as <see cref="InvestmentDto"/>,
    /// or null if not found.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="investmentId"/> is empty.</exception>
    /// <remarks>
    /// This method retrieves only the investment entity without the linked expense.
    /// Use <see cref="GetWithExpenseAsync"/> to include the linked expense details.
    /// </remarks>
    /// <example>
    /// <code>
    /// var investment = await investmentService.GetAsync(investmentId);
    /// if (investment == null)
    /// {
    ///     Console.WriteLine("Investment not found");
    /// }
    /// </code>
    /// </example>
    Task<InvestmentDto?> GetAsync(Guid investmentId);
    
    /// <summary>
    /// Asynchronously retrieves an investment entry with its linked expense details.
    /// </summary>
    /// <param name="investmentId">The unique identifier of the investment to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the investment with linked expense as <see cref="InvestmentDto"/>,
    /// or null if not found.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="investmentId"/> is empty.</exception>
    /// <remarks>
    /// This method eagerly loads the linked expense information if it exists.
    /// </remarks>
    /// <example>
    /// <code>
    /// var investment = await investmentService.GetWithExpenseAsync(investmentId);
    /// if (investment?.LinkedExpense != null)
    /// {
    ///     Console.WriteLine($"Linked to expense: {investment.LinkedExpense.ExpenseName}");
    /// }
    /// </code>
    /// </example>
    Task<InvestmentDto?> GetWithExpenseAsync(Guid investmentId);
    
    /// <summary>
    /// Asynchronously retrieves all investment entries for a specific budget.
    /// </summary>
    /// <param name="budgetId">The unique identifier of the budget.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of all investments
    /// associated with the specified budget, ordered by DisplayOrder.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="budgetId"/> is empty.</exception>
    /// <remarks>
    /// Returns an empty list if no investments are found for the specified budget.
    /// Results are ordered by DisplayOrder in ascending order.
    /// This method does not include linked expenses. Use <see cref="GetAllByBudgetIdWithExpensesAsync"/> for that.
    /// </remarks>
    /// <example>
    /// <code>
    /// var investments = await investmentService.GetAllByBudgetIdAsync(budgetId);
    /// var totalInvestmentValue = investments.Sum(i => i.CurrentValue);
    /// var totalYearlyGrowth = investments.Sum(i => i.EstYearlyGrowth);
    /// </code>
    /// </example>
    Task<List<InvestmentDto>> GetAllByBudgetIdAsync(Guid budgetId);
    /// <summary>
    /// Asynchronously retrieves all investment entries for a specific budget with linked expense details.
    /// </summary>
    /// <param name="budgetId">The unique identifier of the budget.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of all investments
    /// with their linked expenses, ordered by DisplayOrder.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="budgetId"/> is empty.</exception>
    /// <remarks>
    /// This method eagerly loads linked expense information for all investments.
    /// </remarks>
    /// <example>
    /// <code>
    /// var investments = await investmentService.GetAllByBudgetIdWithExpensesAsync(budgetId);
    /// foreach (var investment in investments)
    /// {
    ///     if (investment.LinkedExpense != null)
    ///     {
    ///         Console.WriteLine($"{investment.InvestmentName} → {investment.LinkedExpense.ExpenseName}");
    ///     }
    /// }
    /// </code>
    /// </example>
    Task<List<InvestmentDto>> GetAllByBudgetIdWithExpensesAsync(Guid budgetId);
}