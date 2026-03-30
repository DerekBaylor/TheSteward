using Microsoft.EntityFrameworkCore;
using TheSteward.Core.Dtos.FinanceManagerDtos;
using TheSteward.Core.IRepositories.FinanceManagerIRepositories;
using TheSteward.Core.IServices.FinanceManagerIServices;
using TheSteward.Core.MappingExtensions;

namespace TheSteward.Infrastructure.Services.FinanceManagerServices;

public class CreditService : ICreditService
{
    private readonly ICreditRepository _creditRepository;
    private readonly IBudgetCategoryService _budgetCategoryService;
    private readonly IBudgetSubCategoryService _budgetSubCategoryService;
    private readonly IExpenseService _expenseService;

    public CreditService(ICreditRepository creditRepository, IBudgetCategoryService budgetCategoryService,IBudgetSubCategoryService budgetSubCategoryService, IExpenseService expenseService)
    {
        _creditRepository = creditRepository;
        _budgetCategoryService = budgetCategoryService;
        _budgetSubCategoryService = budgetSubCategoryService;
        _expenseService = expenseService;
    }

    public async Task<CreditDto> AddAsync(CreateCreditDto createCreditDto)
    {
        if (createCreditDto == null)
            throw new ArgumentNullException(nameof(createCreditDto));

        var category = await ResolveCategoryAsync(
            createCreditDto.BudgetId,
            createCreditDto.BudgetCategoryId,
            createCreditDto.BudgetCategoryName);

        var subCategory = await ResolveSubCategoryAsync(
            createCreditDto.BudgetId,
            category.BudgetCategoryId,
            createCreditDto.BudgetSubCategoryId,
            createCreditDto.BudgetSubCategoryName);

        var creditId = Guid.NewGuid();

        var expenseDto = new CreateExpenseDto
        {
            ExpenseName = createCreditDto.CreditName,
            DueDay = createCreditDto.PaymentDay,
            AmountDue = createCreditDto.PaymentAmount,
            DisplayOrder = createCreditDto.DisplayOrder,
            BudgetId = createCreditDto.BudgetId,
            BudgetCategoryId = category.BudgetCategoryId,
            BudgetSubCategoryId = subCategory?.BudgetSubCategoryId,
            CreditId = creditId,
            CreatedByUserHouseholdId = createCreditDto.CreatedByUserHouseholdId,
        };

        var expense = await _expenseService.AddAsync(expenseDto);

        var credit = createCreditDto.ToEntity(creditId, expense.ExpenseId);

        await _creditRepository.AddAsync(credit);
        await _creditRepository.SaveChangesAsync();

        return credit.ToDto();
    }

    public async Task<CreditDto> UpdateAsync(UpdateCreditDto updateCreditDto)
    {
        if (updateCreditDto == null)
            throw new ArgumentNullException(nameof(updateCreditDto));

        var credit = await _creditRepository.GetByIdAsync(updateCreditDto.CreditId);
        if (credit == null)
            throw new KeyNotFoundException($"Credit with ID {updateCreditDto.CreditId} not found.");

        var budgetId = updateCreditDto.BudgetId != Guid.Empty
            ? updateCreditDto.BudgetId
            : credit.BudgetId;

        if (budgetId == Guid.Empty)
            throw new ArgumentException($"BudgetId could not be resolved for credit {updateCreditDto.CreditId}.");

        var category = await ResolveCategoryAsync(
            budgetId,
            updateCreditDto.BudgetCategoryId,
            updateCreditDto.BudgetCategoryName);

        var subCategory = await ResolveSubCategoryAsync(
            budgetId,
            category.BudgetCategoryId,
            updateCreditDto.BudgetSubCategoryId,
            updateCreditDto.BudgetSubCategoryName);

        credit.ApplyUpdate(updateCreditDto);

        await _creditRepository.UpdateAsync(credit);

        if (credit.ExpenseId != Guid.Empty)
        {
            var expense = await _expenseService.GetAsync(credit.ExpenseId);
            if (expense == null)
                throw new KeyNotFoundException(
                    $"Linked expense with ID {credit.ExpenseId} not found for credit {updateCreditDto.CreditId}.");

            var updateExpenseDto = new UpdateExpenseDto
            {
                ExpenseId = expense.ExpenseId,
                ExpenseName = updateCreditDto.CreditName,
                AmountDue = updateCreditDto.PaymentAmount,
                DueDay = updateCreditDto.PaymentDay,
                DisplayOrder = updateCreditDto.DisplayOrder,
                BudgetCategoryId = category.BudgetCategoryId,
                BudgetSubCategoryId = subCategory?.BudgetSubCategoryId,
                CreditId = updateCreditDto.CreditId,
                InvestmentId = expense.InvestmentId,
            };

            await _expenseService.UpdateAsync(updateExpenseDto);
        }

        await _creditRepository.SaveChangesAsync();

        return credit.ToDto();
    }

    public async Task DeleteAsync(Guid creditId)
    {
        if (creditId == Guid.Empty)
            throw new ArgumentException("Credit ID cannot be empty.", nameof(creditId));

        var credit = await _creditRepository.GetByIdAsync(creditId);
        if (credit == null)
            throw new KeyNotFoundException($"Credit with ID {creditId} not found.");

        var expenseId = credit.ExpenseId;

        // Delete the credit first to release the FK, then clean up the expense
        await _creditRepository.DeleteAsync(credit);
        await _creditRepository.SaveChangesAsync();

        if (expenseId != Guid.Empty)
            await _expenseService.DeleteAsync(expenseId);
    }

    public async Task<CreditDto?> GetAsync(Guid creditId)
    {
        if (creditId == Guid.Empty)
            throw new ArgumentException("Credit ID cannot be empty.", nameof(creditId));

        var credit = await _creditRepository.GetByIdAsync(creditId);

        return credit?.ToDto();
    }

    #region Get Methods

    public async Task<CreditDto?> GetWithExpenseAsync(Guid creditId)
    {
        if (creditId == Guid.Empty)
            throw new ArgumentException("Credit ID cannot be empty.", nameof(creditId));

        var credit = await _creditRepository.GetAll()
            .Include(c => c.LinkedExpense)
            .FirstOrDefaultAsync(c => c.CreditId == creditId);

        return credit?.ToDto();
    }

    public async Task<List<CreditDto>> GetAllByBudgetIdAsync(Guid budgetId)
    {
        if (budgetId == Guid.Empty)
            throw new ArgumentException("Budget ID cannot be empty.", nameof(budgetId));

        var credits = await _creditRepository.GetAll()
            .Where(c => c.BudgetId == budgetId)
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();

        return credits.ToDtoList();
    }

    public async Task<List<CreditDto>> GetAllByBudgetIdWithExpensesAsync(Guid budgetId)
    {
        if (budgetId == Guid.Empty)
            throw new ArgumentException("Budget ID cannot be empty.", nameof(budgetId));

        var credits = await _creditRepository.GetAll()
            .Where(c => c.BudgetId == budgetId)
            .Include(c => c.LinkedExpense)
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();

        return credits.ToDtoList();
    }

    #endregion Get Methods

    #region Private Methods

    /// <summary>
    /// Returns an existing category by ID, or gets/creates one by name.
    /// Always returns a valid category — never null.
    /// </summary>
    private async Task<BudgetCategoryDto> ResolveCategoryAsync(
        Guid budgetId, Guid? categoryId, string categoryName)
    {
        if (categoryId.HasValue && categoryId.Value != Guid.Empty)
        {
            var category = await _budgetCategoryService.GetAsync(categoryId.Value);
            if (category == null)
                throw new KeyNotFoundException(
                    $"Budget category with ID {categoryId.Value} not found.");

            return category;
        }

        var fallbackName = string.IsNullOrWhiteSpace(categoryName)
            ? "Debt & Credit"
            : categoryName;

        return await _budgetCategoryService.GetByIdOrCreateAsync(budgetId, fallbackName);
    }

    /// <summary>
    /// Returns an existing subcategory by ID, or gets/creates one by name.
    /// Returns null when no subcategory info is provided at all.
    /// </summary>
    private async Task<BudgetSubCategoryDto?> ResolveSubCategoryAsync(
        Guid budgetId, Guid budgetCategoryId, Guid? subCategoryId, string? subCategoryName)
    {
        if (subCategoryId.HasValue && subCategoryId.Value != Guid.Empty)
        {
            var subCategory = await _budgetSubCategoryService.GetAsync(subCategoryId.Value);
            if (subCategory == null)
                throw new KeyNotFoundException(
                    $"Budget subcategory with ID {subCategoryId.Value} not found.");

            return subCategory;
        }

        if (!string.IsNullOrWhiteSpace(subCategoryName))
            return await _budgetSubCategoryService.GetByIdOrCreateSubCategoryAsync(
                budgetId, budgetCategoryId, subCategoryName);

        return null;
    }

    #endregion Private Methods
}