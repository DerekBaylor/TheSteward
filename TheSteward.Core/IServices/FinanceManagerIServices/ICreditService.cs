using TheSteward.Core.Dtos.FinanceManagerDtos;
using TheSteward.Core.Models.FinanceManagerModels;

namespace TheSteward.Core.IServices.FinanceManagerIServices;

public interface ICreditService
{
    /// <summary>
    /// Creates a new credit account, resolves or creates the associated budget category and
    /// optional subcategory, and generates a linked expense to represent the recurring payment
    /// within the budget.
    /// </summary>
    /// <remarks>
    /// The linked expense is created with the same name, payment amount, and due day as the
    /// credit. After the expense is persisted, its ID is stamped back onto the credit record so
    /// the two remain associated. If <see cref="CreateCreditDto.BudgetCategoryId"/> is supplied
    /// it is used directly; otherwise the service will get-or-create a category by
    /// <see cref="CreateCreditDto.BudgetCategoryName"/>, defaulting to <c>"Debt &amp; Credit"</c>
    /// when neither is provided. The same get-or-create logic applies to the optional subcategory.
    /// </remarks>
    /// <param name="dto">The data required to create the credit.</param>
    /// <returns>A <see cref="CreditDto"/> representing the newly created credit.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dto"/> is null.</exception>
    /// <exception cref="KeyNotFoundException">
    /// Thrown when a <see cref="CreateCreditDto.BudgetCategoryId"/> or
    /// <see cref="CreateCreditDto.BudgetSubCategoryId"/> is supplied but cannot be found.
    /// </exception>
    Task<CreditDto> AddAsync(CreateCreditDto creditDto);

    /// <summary>
    /// Updates an existing credit account and synchronises all changes to its linked expense.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The following fields are always written through to the linked expense when a valid
    /// <see cref="UpdateCreditDto.ExpenseId"/> is present on the credit record:
    /// <list type="bullet">
    ///   <item><description>
    ///     <b>Amount due</b> — kept in sync with <see cref="UpdateCreditDto.PaymentAmount"/>.
    ///   </description></item>
    ///   <item><description>
    ///     <b>Due day</b> — kept in sync with <see cref="UpdateCreditDto.PaymentDay"/>.
    ///   </description></item>
    ///   <item><description>
    ///     <b>Budget category</b> — resolved via <see cref="UpdateCreditDto.BudgetCategoryId"/>
    ///     when supplied, otherwise gets-or-creates by <see cref="UpdateCreditDto.BudgetCategoryName"/>.
    ///   </description></item>
    ///   <item><description>
    ///     <b>Budget subcategory</b> — resolved via <see cref="UpdateCreditDto.BudgetSubCategoryId"/>
    ///     when supplied, gets-or-creates by <see cref="UpdateCreditDto.BudgetSubCategoryName"/>
    ///     when a name is given, or cleared when both are null.
    ///   </description></item>
    /// </list>
    /// </para>
    /// <para>
    /// If the credit has no linked expense the expense update step is skipped and only
    /// the credit itself is saved.
    /// </para>
    /// </remarks>
    /// <param name="dto">
    /// The updated credit data. Must include a valid <see cref="UpdateCreditDto.CreditId"/>.
    /// </param>
    /// <returns>The same <paramref name="dto"/> passed in, after a successful update.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dto"/> is null.</exception>
    /// <exception cref="KeyNotFoundException">
    /// Thrown when no credit exists with <see cref="UpdateCreditDto.CreditId"/>, or when a
    /// linked expense ID is recorded on the credit but that expense cannot be found.
    /// </exception>
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
    Task<List<CreditDto>> GetAllByBudgetIdWithExpensesAsync(Guid budgetId);
}