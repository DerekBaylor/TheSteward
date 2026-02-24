using TheSteward.Core.Dtos.FinanceManagerDtos;
using TheSteward.Core.Models.FinanceManagerModels;

namespace TheSteward.Core.IServices.FinanceManagerIServices;

public interface IIncomeService
{
    /// <summary>
    /// Asynchronously creates a new income entry with calculated tax and salary values.
    /// </summary>
    /// <param name="incomeDto">The income data for creation.</param>
    /// <returns>
    /// A task result contains the created income as <see cref="IncomeDto"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="incomeDto"/> is null.</exception>
    /// <remarks>
    /// This method calculates the following values automatically:
    /// <list type="bullet">
    /// <item><description>YearlyGrossSalary = PayCheckGross × IncomeFrequency</description></item>
    /// <item><description>EstFederalIncomeTax (based on yearly salary and standard deduction)</description></item>
    /// <item><description>EstStateIncomeTax (state-specific calculation, if applicable)</description></item>
    /// <item><description>MonthlyNetIncome (gross monthly income minus taxes)</description></item>
    /// </list>
    /// </remarks>
    /// <example>
    /// <code>
    /// var createDto = new CreateIncomeDto
    /// {
    ///     IncomeName = "Primary Job",
    ///     IncomeFrequency = 26, // Bi-weekly
    ///     PayCheckGross = 2500m,
    ///     BudgetId = budgetId
    /// };
    /// var income = await incomeService.AddAsync(createDto);
    /// </code>
    /// </example>
    Task<IncomeDto> AddAsync(CreateIncomeDto  incomeDto);
    
    /// <summary>
    /// Asynchronously deletes an income entry by its identifier.
    /// </summary>
    /// <param name="incomeId">The unique identifier of the income to delete.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="incomeId"/> is empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the income with the specified ID is not found.</exception>
    /// <remarks>
    /// This performs a hard delete, permanently removing the income from the database.
    /// </remarks>
    /// <example>
    /// <code>
    /// await incomeService.DeleteAsync(incomeId);
    /// </code>
    /// </example>
    Task DeleteAsync(Guid incomeId);
    
    /// <summary>
    /// Asynchronously updates an existing income entry with recalculated values.
    /// </summary>
    /// <param name="incomeDto">The income data with updated values.</param>
    /// <returns>
    /// A task result contains the updated income as <see cref="UpdateIncomeDto"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="incomeDto"/> is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the income with the specified ID is not found.</exception>
    /// <remarks>
    /// This method recalculates all derived values (yearly salary, taxes, monthly net income) 
    /// based on the updated PayCheckGross and IncomeFrequency values.
    /// </remarks>
    /// <example>
    /// <code>
    /// var updateDto = new UpdateIncomeDto
    /// {
    ///     IncomeId = incomeId,
    ///     IncomeName = "Primary Job - Updated",
    ///     IncomeFrequency = 24, // Changed to bi-monthly
    ///     PayCheckGross = 2700m, // Raise!
    ///     DisplayOrder = 1
    /// };
    /// var updatedIncome = await incomeService.UpdateAsync(updateDto);
    /// </code>
    /// </example>
    Task<UpdateIncomeDto> UpdateAsync(UpdateIncomeDto incomeDto);
    
    /// <summary>
    /// Asynchronously retrieves a single income entry by its identifier.
    /// </summary>
    /// <param name="incomeId">The unique identifier of the income to retrieve.</param>
    /// <returns>
    /// A task result contains the income as <see cref="IncomeDto"/>,
    /// or null if not found.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="incomeId"/> is empty.</exception>
    /// <example>
    /// <code>
    /// var income = await incomeService.GetAsync(incomeId);
    /// if (income == null)
    /// {
    ///     Console.WriteLine("Income not found");
    /// }
    /// </code>
    /// </example>
    Task<IncomeDto> GetAsync(Guid incomeId);
    
    /// <summary>
    /// Asynchronously retrieves all income entries for a specific budget.
    /// </summary>
    /// <param name="budgetId">The unique identifier of the budget.</param>
    /// <returns>
    /// A task result contains a list of all incomes
    /// associated with the specified budget, ordered by DisplayOrder.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="budgetId"/> is empty.</exception>
    /// <remarks>
    /// Returns an empty list if no incomes are found for the specified budget.
    /// Results are ordered by DisplayOrder in ascending order.
    /// </remarks>
    /// <example>
    /// <code>
    /// var incomes = await incomeService.GetAllByBudgetIdAsync(budgetId);
    /// var totalMonthlyIncome = incomes.Sum(i => i.MonthlyNetIncome);
    /// </code>
    /// </example>
    Task<List<IncomeDto>> GetAllByBudgetIdAsync(Guid budgetId);
}