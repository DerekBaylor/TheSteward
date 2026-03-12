using AutoMapper;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using TheSteward.Core.Dtos.FinanceManagerDtos;
using TheSteward.Core.IRepositories.FinanceManagerIRepositories;
using TheSteward.Core.IServices.FinanceManagerIServices;
using TheSteward.Core.Models.FinanceManagerModels;

namespace TheSteward.Infrastructure.Services.FinanceManagerServices;

public class CreditService : ICreditService
{
    private readonly ICreditRepository _creditRepository;
    private readonly IBudgetCategoryService _budgetCategoryService;
    private readonly IBudgetSubCategoryService _budgetSubCategoryService;
    private readonly IExpenseService _expenseService;
    private readonly IMapper _mapper;

    public CreditService(ICreditRepository creditRepository, IBudgetCategoryService budgetCategoryService, IBudgetSubCategoryService budgetSubCategoryService, IExpenseService expenseService, IMapper mapper) 
    {
        _creditRepository = creditRepository;
        _budgetCategoryService = budgetCategoryService;
        _budgetSubCategoryService = budgetSubCategoryService;
        _expenseService = expenseService;
        _mapper = mapper;
    }

    public async Task<CreditDto> AddAsync(CreateCreditDto createCreditDto)
    {
        if (createCreditDto == null)
            throw new ArgumentNullException(nameof(createCreditDto));

        // --- Resolve category & subcategory ---
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

        // --- Create the linked expense ---
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
        };

        var expense = await _expenseService.AddAsync(expenseDto);

        var credit = new Credit
        {
            CreditId = creditId,
            CreditName = createCreditDto.CreditName,
            CreditType = createCreditDto.CreditType,
            InterestRate = createCreditDto.InterestRate,
            CurrentValue = createCreditDto.CurrentValue,
            EstMonthlyInterest = createCreditDto.EstMonthlyInterest,
            EstYearlyInterest = createCreditDto.EstYearlyInterest,
            PaymentFrequency = createCreditDto.PaymentFrequency,
            PaymentAmount = createCreditDto.PaymentAmount,
            PaymentDay = createCreditDto.PaymentDay,
            DisplayOrder = createCreditDto.DisplayOrder,
            BudgetId = createCreditDto.BudgetId,
            ExpenseId = expense.ExpenseId,
        };

        await _creditRepository.AddAsync(credit);
        await _creditRepository.SaveChangesAsync();

        var creditDto = _mapper.Map<CreditDto>(credit);
        return creditDto;
    }

    public async Task<UpdateCreditDto> UpdateAsync(UpdateCreditDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        var credit = await _creditRepository.GetByIdAsync(dto.CreditId);

        if (credit == null)
            throw new KeyNotFoundException($"Credit with ID {dto.CreditId} not found.");

        var budgetId = dto.BudgetId != Guid.Empty ? dto.BudgetId : credit.BudgetId;

        if (budgetId == Guid.Empty)
            throw new ArgumentException($"BudgetId could not be resolved for credit {dto.CreditId}.");

        // --- Resolve category & subcategory (mirrors AddAsync) ---
        var category = await ResolveCategoryAsync(budgetId, dto.BudgetCategoryId, dto.BudgetCategoryName);

        var subCategory = await ResolveSubCategoryAsync(budgetId, category.BudgetCategoryId, dto.BudgetSubCategoryId, dto.BudgetSubCategoryName);

        Console.WriteLine($"[CreditService.UpdateAsync] Resolved CategoryId: {category.BudgetCategoryId}");
        Console.WriteLine($"[CreditService.UpdateAsync] Resolved CategoryName: {category.BudgetCategoryName}");


        // --- Update the credit ---
        credit.CreditName = dto.CreditName;
        credit.CreditType = dto.CreditType;
        credit.InterestRate = dto.InterestRate;
        credit.CurrentValue = dto.CurrentValue;
        credit.EstMonthlyInterest = dto.EstMonthlyInterest;
        credit.EstYearlyInterest = dto.EstYearlyInterest;
        credit.PaymentFrequency = dto.PaymentFrequency;
        credit.PaymentAmount = dto.PaymentAmount;
        credit.PaymentDay = dto.PaymentDay;
        credit.DisplayOrder = dto.DisplayOrder;

        await _creditRepository.UpdateAsync(credit);

        // --- Sync the linked expense (if one exists) ---
        if (credit.ExpenseId != Guid.Empty)
        {
            var expense = await _expenseService.GetAsync(credit.ExpenseId);

            if (expense == null)
                throw new KeyNotFoundException($"Linked expense with ID {credit.ExpenseId} not found for credit {dto.CreditId}.");

            var updateExpenseDto = new UpdateExpenseDto
            {
                ExpenseId = expense.ExpenseId,
                ExpenseName = dto.CreditName,
                AmountDue = dto.PaymentAmount,
                DueDay = dto.PaymentDay,
                DisplayOrder = dto.DisplayOrder,
                BudgetCategoryId = category.BudgetCategoryId,
                BudgetSubCategoryId = subCategory?.BudgetSubCategoryId,
                CreditId = dto.CreditId,
                InvestmentId = expense.InvestmentId,
            };

            await _expenseService.UpdateAsync(updateExpenseDto);
        }

        await _creditRepository.SaveChangesAsync();

        return dto;
    }

    public async Task DeleteAsync(Guid creditId)
    {
        if (creditId == Guid.Empty)
            throw new ArgumentException("Credit ID cannot be empty.", nameof(creditId));

        var credit = await _creditRepository.GetByIdAsync(creditId);
        if (credit == null)
            throw new KeyNotFoundException($"Credit with ID {creditId} not found.");

        await _creditRepository.DeleteAsync(credit);
        await _creditRepository.SaveChangesAsync();
    }
    public async Task<CreditDto?> GetAsync(Guid creditId)
    {
        if (creditId == Guid.Empty)
            throw new ArgumentException("Credit ID cannot be empty.", nameof(creditId));

        var credit = await _creditRepository.GetByIdAsync(creditId);

        return credit == null ? null : _mapper.Map<CreditDto>(credit);
    }

    #region Get Methods
    public async Task<CreditDto?> GetWithExpenseAsync(Guid creditId)
    {
        if (creditId == Guid.Empty)
            throw new ArgumentException("Credit ID cannot be empty.", nameof(creditId));

        var credit = await _creditRepository.GetAll()
            .Include(c => c.LinkedExpense)
            .FirstOrDefaultAsync(c => c.CreditId == creditId);

        return credit == null ? null : _mapper.Map<CreditDto>(credit);
    }

    public async Task<List<CreditDto>> GetAllByBudgetIdAsync(Guid budgetId)
    {
        if (budgetId == Guid.Empty)
            throw new ArgumentException("Budget ID cannot be empty.", nameof(budgetId));

        var credits = await _creditRepository.GetAll()
            .Where(c => c.BudgetId == budgetId)
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();

        return _mapper.Map<List<CreditDto>>(credits);
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

        return _mapper.Map<List<CreditDto>>(credits);
    }

    #endregion Get Methods

    #region Private Methods

    /// <summary>
    /// Returns an existing category by ID, or gets/creates one by name.
    /// Always returns a valid category — never null.
    /// </summary>
    private async Task<BudgetCategoryDto> ResolveCategoryAsync(Guid budgetId, Guid? categoryId, string categoryName)
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

        var budgetCategoryDto = await _budgetCategoryService.GetByIdOrCreateAsync(budgetId, fallbackName);
        
        return budgetCategoryDto;
    }

    /// <summary>
    /// Returns an existing subcategory by ID, or gets/creates one by name.
    /// Returns null when no subcategory info is provided at all.
    /// </summary>
    private async Task<BudgetSubCategoryDto?> ResolveSubCategoryAsync(Guid budgetId, Guid budgetCategoryId, Guid? subCategoryId, string? subCategoryName)
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
        {
            return await _budgetSubCategoryService.GetByIdOrCreateSubCategoryAsync(
                budgetId,
                budgetCategoryId,
                subCategoryName);
        }

        return null;
    }
    #endregion Private Methods
}