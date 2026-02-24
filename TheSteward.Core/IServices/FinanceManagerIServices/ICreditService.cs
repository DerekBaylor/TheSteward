using TheSteward.Core.Dtos.FinanceManagerDtos;
using TheSteward.Core.Models.FinanceManagerModels;

namespace TheSteward.Core.IServices.FinanceManagerIServices;

public interface ICreditService
{
    /// <summary>
    /// Asynchronously creates a new credit entry.
    /// </summary>
    /// <param name="creditDto">The credit data for creation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the created credit as <see cref="CreditDto"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="creditDto"/> is null.</exception>
    /// <remarks>
    /// Interest calculations (EstMonthlyInterest and EstYearlyInterest) should be performed on the frontend
    /// before calling this method.
    /// </remarks>
    /// <example>
    /// <code>
    /// var createDto = new CreateCreditDto
    /// {
    ///     CreditName = "Chase Sapphire",
    ///     CreditType = "Credit Card",
    ///     InterestRate = 0.1999m, // 19.99%
    ///     CurrentValue = 5000m,
    ///     EstMonthlyInterest = 83.29m, // Calculated on frontend
    ///     EstYearlyInterest = 999.50m, // Calculated on frontend
    ///     PaymentFrequency = 12, // Monthly
    ///     PaymentAmount = 150m,
    ///     PaymentDay = 15,
    ///     BudgetId = budgetId
    /// };
    /// var credit = await creditService.AddAsync(createDto);
    /// </code>
    /// </example>
    Task<CreditDto> AddAsync(CreateCreditDto creditDto);

    /// <summary>
    /// Asynchronously updates an existing credit entry.
    /// </summary>
    /// <param name="creditDto">The credit data with updated values.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the updated credit as <see cref="UpdateCreditDto"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="creditDto"/> is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the credit with the specified ID is not found.</exception>
    /// <remarks>
    /// Interest calculations (EstMonthlyInterest and EstYearlyInterest) should be performed on the frontend
    /// before calling this method.
    /// </remarks>
    /// <example>
    /// <code>
    /// var updateDto = new UpdateCreditDto
    /// {
    ///     CreditId = creditId,
    ///     CreditName = "Chase Sapphire Reserve",
    ///     InterestRate = 0.2199m, // Rate increased
    ///     CurrentValue = 4500m, // Balance decreased
    ///     EstMonthlyInterest = 82.46m, // Recalculated on frontend
    ///     EstYearlyInterest = 989.55m, // Recalculated on frontend
    ///     PaymentAmount = 200m, // Increased payment
    ///     // ... other fields
    /// };
    /// var updated = await creditService.UpdateAsync(updateDto);
    /// </code>
    /// </example>
    Task<UpdateCreditDto> UpdateAsync(UpdateCreditDto creditDto);

    /// <summary>
    /// Asynchronously deletes a credit entry by its identifier.
    /// </summary>
    /// <param name="creditId">The unique identifier of the credit to delete.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="creditId"/> is empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the credit with the specified ID is not found.</exception>
    /// <remarks>
    /// This performs a hard delete, permanently removing the credit from the database.
    /// Consider the impact on linked expenses before deleting.
    /// </remarks>
    /// <example>
    /// <code>
    /// await creditService.DeleteAsync(creditId);
    /// </code>
    /// </example>
    Task DeleteAsync(Guid creditId);

    /// <summary>
    /// Asynchronously retrieves a single credit entry by its identifier.
    /// </summary>
    /// <param name="creditId">The unique identifier of the credit to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the credit as <see cref="CreditDto"/>,
    /// or null if not found.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="creditId"/> is empty.</exception>
    /// <example>
    /// <code>
    /// var credit = await creditService.GetAsync(creditId);
    /// if (credit == null)
    /// {
    ///     Console.WriteLine("Credit not found");
    /// }
    /// </code>
    /// </example>
    Task<CreditDto?> GetAsync(Guid creditId);

    /// <summary>
    /// Asynchronously retrieves a credit entry with its linked expense details.
    /// </summary>
    /// <param name="creditId">The unique identifier of the credit to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the credit with linked expense as <see cref="CreditDto"/>,
    /// or null if not found.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="creditId"/> is empty.</exception>
    /// <remarks>
    /// This method eagerly loads the linked expense information if it exists.
    /// </remarks>
    /// <example>
    /// <code>
    /// var credit = await creditService.GetWithExpenseAsync(creditId);
    /// if (credit?.LinkedExpense != null)
    /// {
    ///     Console.WriteLine($"Linked to expense: {credit.LinkedExpense.ExpenseName}");
    /// }
    /// </code>
    /// </example>
    Task<CreditDto?> GetWithExpenseAsync(Guid creditId);

    /// <summary>
    /// Asynchronously retrieves all credit entries for a specific budget.
    /// </summary>
    /// <param name="budgetId">The unique identifier of the budget.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of all credits
    /// associated with the specified budget, ordered by DisplayOrder.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="budgetId"/> is empty.</exception>
    /// <remarks>
    /// Returns an empty list if no credits are found for the specified budget.
    /// Results are ordered by DisplayOrder in ascending order.
    /// </remarks>
    /// <example>
    /// <code>
    /// var credits = await creditService.GetAllByBudgetIdAsync(budgetId);
    /// var totalDebt = credits.Sum(c => c.CurrentValue);
    /// var totalMonthlyInterest = credits.Sum(c => c.EstMonthlyInterest);
    /// </code>
    /// </example>
    Task<List<CreditDto>> GetAllByBudgetIdAsync(Guid budgetId);

    /// <summary>
    /// Asynchronously retrieves all credit entries for a specific budget with linked expense details.
    /// </summary>
    /// <param name="budgetId">The unique identifier of the budget.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of all credits
    /// with their linked expenses, ordered by DisplayOrder.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="budgetId"/> is empty.</exception>
    /// <example>
    /// <code>
    /// var credits = await creditService.GetAllByBudgetIdWithExpensesAsync(budgetId);
    /// foreach (var credit in credits)
    /// {
    ///     if (credit.LinkedExpense != null)
    ///     {
    ///         Console.WriteLine($"{credit.CreditName} → {credit.LinkedExpense.ExpenseName}");
    ///     }
    /// }
    /// </code>
    /// </example>
    Task<List<CreditDto>> GetAllByBudgetIdWithExpensesAsync(Guid budgetId);
}