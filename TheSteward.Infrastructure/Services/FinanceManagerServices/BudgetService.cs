//01/20/26
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TheSteward.Core.Dtos.FinanceManagerDtos;
using TheSteward.Core.Dtos.HouseholdDtos;
using TheSteward.Core.IRepositories.FinanceManagerIRepositories;
using TheSteward.Core.IServices.FinanceManagerIServices;
using TheSteward.Core.Models.FinanceManagerModels;
using TheSteward.Core.Utils.FinanceManagerUtils;
using static TheSteward.Core.Utils.FinanceManagerUtils.FinanceManagerConstants;


namespace TheSteward.Infrastructure.Services.FinanceManagerServices;

public class BudgetService : IBudgetService
{
    private readonly IBudgetRepository _budgetRepository;
    private readonly IBudgetCategoryRepository _categoryRepository;
    private readonly IBudgetSubCategoryRepository _subCategoryRepository;
    private readonly ICreditRepository _creditRepository;
    private readonly IExpenseRepository _expenseRepository;
    private readonly IIncomeRepository _incomeRepository;
    private readonly IInvestmentRepository _investmentRepository;
    private readonly IMapper _mapper;

    public BudgetService(
        IBudgetRepository budgetRepository,
        IBudgetCategoryRepository categoryRepository,
        IBudgetSubCategoryRepository subCategoryRepository,
        ICreditRepository creditRepository,
        IExpenseRepository expenseRepository,
        IIncomeRepository incomeRepository,
        IInvestmentRepository investmentRepository,
        IMapper mapper)
    {
        _budgetRepository = budgetRepository;
        _categoryRepository = categoryRepository;
        _subCategoryRepository = subCategoryRepository;
        _expenseRepository = expenseRepository;
        _creditRepository = creditRepository;
        _incomeRepository = incomeRepository;
        _investmentRepository =  investmentRepository;
        _mapper = mapper;
    }

    #region Create Methods
    public async Task<BudgetDto> AddAsync(CreateBudgetDto budgetDto)
    {
        if (budgetDto == null)
            throw new ArgumentNullException(nameof(budgetDto));

        // If this is set as default, unset any existing default budgets for this household
        if (budgetDto.IsDefaultBudget)
        {
            await UnsetDefaultBudgetsForHouseholdAsync(budgetDto.HouseholdId);
        }

        var budget = new Budget
        {
            BudgetId = Guid.NewGuid(),
            BudgetName = budgetDto.BudgetName,
            OwnerId = budgetDto.OwnerId,
            HouseholdId = budgetDto.HouseholdId,
            IsDefaultBudget = budgetDto.IsDefaultBudget,
            IsPublic = budgetDto.IsPublic,
            CreatedDate = DateTime.UtcNow
        };

        await _budgetRepository.AddAsync(budget);
        await _budgetRepository.SaveChangesAsync();

        return _mapper.Map<BudgetDto>(budget);
    }

    public async Task<BudgetDto> AddStarterBudget(UserHouseholdDto userHouseholdDto)
    {
        if (userHouseholdDto.UserHouseholdId == Guid.Empty)
            throw new ArgumentException("Household ID cannot be empty.", nameof(userHouseholdDto.UserHouseholdId));

        await using var transaction = await _budgetRepository.BeginTransactionAsync();

        try
        {
            var budget = new Budget
            {
                BudgetId = Guid.NewGuid(),
                BudgetName = "My First Budget",
                HouseholdId = userHouseholdDto.HouseholdId,
                OwnerId = userHouseholdDto.UserId,
                IsDefaultBudget = true,
                IsPublic = false,
                CreatedDate = DateTime.UtcNow
            };

            await _budgetRepository.AddAsync(budget);
            await _budgetRepository.SaveChangesAsync();

            await CreateStandardCategoriesAsync(budget.BudgetId);
            await CreateSampleExpensesAsync(budget.BudgetId);
            await CreateSampleIncomeAsync(budget.BudgetId);
            await CreateSampleCreditAsync(budget.BudgetId);
            await CreateSampleInvestmentAsync(budget.BudgetId);

            await transaction.CommitAsync();

            return await GetByIdAsync(budget.BudgetId);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    #endregion Create Methods
    public async Task DeleteAsync(Guid budgetId)
    {
        if (budgetId == Guid.Empty)
            throw new ArgumentException("Budget ID cannot be empty.", nameof(budgetId));

        var budget = await _budgetRepository.GetByIdAsync(budgetId);
        if (budget == null)
            throw new KeyNotFoundException($"Budget with ID {budgetId} not found.");

        await _budgetRepository.DeleteAsync(budget);
        await _budgetRepository.SaveChangesAsync();
    }

    public async Task<BudgetDto> UpdateAsync(UpdateBudgetDto budgetDto)
    {
        if (budgetDto == null)
            throw new ArgumentNullException(nameof(budgetDto));

        var budget = await _budgetRepository.GetByIdAsync(budgetDto.BudgetId);
        if (budget == null)
            throw new KeyNotFoundException($"Budget with ID {budgetDto.BudgetId} not found.");

        // If changing to default, unset other default budgets
        if (budgetDto.IsDefaultBudget && !budget.IsDefaultBudget)
        {
            await UnsetDefaultBudgetsForHouseholdAsync(budget.HouseholdId);
        }

        budget.BudgetName = budgetDto.BudgetName;
        budget.IsDefaultBudget = budgetDto.IsDefaultBudget;
        budget.IsPublic = budgetDto.IsPublic;
        budget.ModifiedDate = DateTime.UtcNow;

        await _budgetRepository.UpdateAsync(budget);
        await _budgetRepository.SaveChangesAsync();

        return _mapper.Map<BudgetDto>(budget);
    }

    #region Get Methods
    public async Task<BudgetDto> GetByIdAsync(Guid budgetId)
    {
        if (budgetId == Guid.Empty)
            throw new ArgumentException("Budget ID cannot be empty.", nameof(budgetId));

        var budget = await _budgetRepository.GetAll()
            .Include(b => b.BudgetCategories)
            .ThenInclude(c => c.BudgetSubCategories)
            .Include(b => b.Incomes)
            .Include(b => b.Expenses)
            .Include(b => b.Credits)
            .Include(b => b.Investments)
            .AsSplitQuery()
            .FirstOrDefaultAsync(b => b.BudgetId == budgetId);

        if (budget == null)
            throw new KeyNotFoundException($"Budget with ID {budgetId} not found.");

        return _mapper.Map<BudgetDto>(budget);
    }

    public async Task<List<BudgetDto>> GetBudgetsByUserHouseholdIdAsync(Guid userHouseholdId)
    {
        if (userHouseholdId == Guid.Empty)
            throw new ArgumentException("Household ID cannot be empty.", nameof(userHouseholdId));

        var budgets = await _budgetRepository.GetAll()
            .Where(b => b.HouseholdId == userHouseholdId)
            .OrderByDescending(b => b.IsDefaultBudget)
            .ThenByDescending(b => b.CreatedDate)
            .ToListAsync();

        return _mapper.Map<List<BudgetDto>>(budgets);
    }

    public async Task<List<BudgetDto>> GetByHouseholdAsync(Guid householdId)
    {
        if (householdId == Guid.Empty)
            throw new ArgumentException("Household ID cannot be empty.", nameof(householdId));

        var budgets = await _budgetRepository.GetAll()
            .Where(b => b.HouseholdId == householdId)
            .OrderByDescending(b => b.IsDefaultBudget)
            .ThenByDescending(b => b.CreatedDate)
            .ToListAsync();

        return _mapper.Map<List<BudgetDto>>(budgets);
    }

    public async Task<List<BudgetDto>> GetBudgetsForUserHouseholdAsync(UserHouseholdDto userHouseholdDto)
    {
        if (userHouseholdDto.UserHouseholdId == Guid.Empty)
            throw new ArgumentException("UserHousehold ID cannot be empty.", nameof(userHouseholdDto.UserHouseholdId));

        var budgets = await _budgetRepository.GetAll()
            .Where(b => b.HouseholdId == userHouseholdDto.HouseholdId
                        && (b.OwnerId == userHouseholdDto.UserId || b.IsPublic))
            .Include(b => b.BudgetCategories)
                .ThenInclude(c => c.BudgetSubCategories)
            .Include(b => b.Incomes)
            .Include(b => b.Expenses)
            .Include(b => b.Credits)
            .Include(b => b.Investments)
            .AsSplitQuery()
            .OrderByDescending(b => b.BudgetId == userHouseholdDto.DefaultBudgetId)
            .ThenByDescending(b => b.IsDefaultBudget)
            .ThenByDescending(b => b.CreatedDate)
            .ToListAsync();

        return _mapper.Map<List<BudgetDto>>(budgets);
    }

    #endregion Get Methods

    #region Private Helper Methods

    /// <summary>
    /// Unsets the default flag for all budgets in a household.
    /// </summary>
    private async Task UnsetDefaultBudgetsForHouseholdAsync(Guid householdId)
    {
        var existingDefaults = await _budgetRepository.GetAll()
            .Where(b => b.HouseholdId == householdId && b.IsDefaultBudget)
            .ToListAsync();

        foreach (var budget in existingDefaults)
        {
            budget.IsDefaultBudget = false;
            await _budgetRepository.UpdateAsync(budget);
        }
    }

    #endregion Private Helper Methods

    #region Private Methods for Starter Budget Creation

    /// <summary>
    /// Creates standard budget categories and subcategories.
    /// </summary>
    private async Task CreateStandardCategoriesAsync(Guid budgetId)
    {
        var categories = new[]
        {
        ("Housing",         new[] { "House/Apt" }),
        ("Utilities",       Array.Empty<string>()),
        ("Transportation",  new[] { "Car One" }),
        ("Living Expenses", new[] { "Food", "Personal Care" }),
        ("Entertainment",   new[] { "Streaming Services", "Hobbies" }),
        ("Pets",            Array.Empty<string>()),
        ("Health",          Array.Empty<string>()),
        ("Savings",         new[] { "Emergency Fund", "Retirement", "Investments" }),
        ("Debts",           new[] { "Credit Cards", "Student Loans" }),
        ("Child",           Array.Empty<string>()),
        ("Other",           Array.Empty<string>()),
    };

        for (int i = 0; i < categories.Length; i++)
        {
            var (categoryName, subCategoryNames) = categories[i];

            var category = new BudgetCategory
            {
                BudgetCategoryId = Guid.NewGuid(),
                BudgetCategoryName = categoryName,
                BudgetId = budgetId,
                DisplayOrder = i + 1
            };

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();

            for (int j = 0; j < subCategoryNames.Length; j++)
            {
                var subCategory = new BudgetSubCategory
                {
                    BudgetSubCategoryId = Guid.NewGuid(),
                    BudgetSubCategoryName = subCategoryNames[j],
                    BudgetId = budgetId,
                    BudgetCategoryId = category.BudgetCategoryId,
                    DisplayOrder = j + 1
                };

                await _subCategoryRepository.AddAsync(subCategory);
                await _subCategoryRepository.SaveChangesAsync();
            }
        }
    }

    /// <summary>
    /// Creates a sample income entry.
    /// </summary>
    private async Task CreateSampleIncomeAsync(Guid budgetId)
    {
        var income = new Income
        {
            IncomeId = Guid.NewGuid(),
            IncomeName = "Primary Job",
            FilingStatus = FilingStatusEnum.Single,
            IncomeFrequency = FrequencyEnum.BiWeekly,
            PayCheckGross = 2000m,
            YearlyGrossSalary = 52000m,
            EstFederalIncomeTax = 6240m,
            EstStateIncomeTax = 2600m,
            MonthlyNetIncome = 3580m,
            DisplayOrder = 1,
            BudgetId = budgetId
        };

        await _incomeRepository.AddAsync(income);
        await _incomeRepository.SaveChangesAsync();
    }

    /// <summary>
    /// Creates sample expense entries.
    /// </summary>
    private async Task CreateSampleExpensesAsync(Guid budgetId)
    {
        var allCategories = await _categoryRepository.GetAll()
            .Where(c => c.BudgetId == budgetId)
            .Include(c => c.BudgetSubCategories)
            .ToListAsync();

        BudgetCategory? GetCategory(string name) =>
            allCategories.FirstOrDefault(c => c.BudgetCategoryName == name);

        BudgetSubCategory? GetSubCategory(string categoryName, string subName) =>
            GetCategory(categoryName)?.BudgetSubCategories.FirstOrDefault(sc => sc.BudgetSubCategoryName == subName);

        int order = 1;

        async Task AddExpense(Expense expense)
        {
            await _expenseRepository.AddAsync(expense);
            await _expenseRepository.SaveChangesAsync();
        }

        var housingCategory = GetCategory("Housing");
        var houseAptSub = GetSubCategory("Housing", "House/Apt");
        var utilitiesCategory = GetCategory("Utilities");
        var transportCategory = GetCategory("Transportation");
        var carOneSub = GetSubCategory("Transportation", "Car One");
        var livingCategory = GetCategory("Living Expenses");
        var entertainmentCategory = GetCategory("Entertainment");
        var debtsCategory = GetCategory("Debts");
        var creditCardsSub = GetSubCategory("Debts", "Credit Cards");
        var savingsCategory = GetCategory("Savings");
        var emergencyFundSub = GetSubCategory("Savings", "Emergency Fund");

        if (housingCategory != null)
        {
            await AddExpense(new Expense { ExpenseId = Guid.NewGuid(), ExpenseName = "Rent", DueDay = 1, AmountDue = 1200m, DisplayOrder = order++, BudgetId = budgetId, BudgetCategoryId = housingCategory.BudgetCategoryId, BudgetSubCategoryId = houseAptSub?.BudgetSubCategoryId });
            await AddExpense(new Expense { ExpenseId = Guid.NewGuid(), ExpenseName = "Renter's Insurance", DueDay = 1, AmountDue = 15m, DisplayOrder = order++, BudgetId = budgetId, BudgetCategoryId = housingCategory.BudgetCategoryId, BudgetSubCategoryId = houseAptSub?.BudgetSubCategoryId });
        }

        if (utilitiesCategory != null)
        {
            await AddExpense(new Expense { ExpenseId = Guid.NewGuid(), ExpenseName = "Electric Bill", DueDay = 15, AmountDue = 100m, DisplayOrder = order++, BudgetId = budgetId, BudgetCategoryId = utilitiesCategory.BudgetCategoryId, BudgetSubCategoryId = null });
            await AddExpense(new Expense { ExpenseId = Guid.NewGuid(), ExpenseName = "Internet Service", DueDay = 20, AmountDue = 70m, DisplayOrder = order++, BudgetId = budgetId, BudgetCategoryId = utilitiesCategory.BudgetCategoryId, BudgetSubCategoryId = null });
        }

        if (transportCategory != null)
        {
            await AddExpense(new Expense { ExpenseId = Guid.NewGuid(), ExpenseName = "Car Payment", DueDay = 5, AmountDue = 350m, DisplayOrder = order++, BudgetId = budgetId, BudgetCategoryId = transportCategory.BudgetCategoryId, BudgetSubCategoryId = carOneSub?.BudgetSubCategoryId });
            await AddExpense(new Expense { ExpenseId = Guid.NewGuid(), ExpenseName = "Car Insurance", DueDay = 10, AmountDue = 120m, DisplayOrder = order++, BudgetId = budgetId, BudgetCategoryId = transportCategory.BudgetCategoryId, BudgetSubCategoryId = carOneSub?.BudgetSubCategoryId });
            await AddExpense(new Expense { ExpenseId = Guid.NewGuid(), ExpenseName = "Gasoline", DueDay = 25, AmountDue = 150m, DisplayOrder = order++, BudgetId = budgetId, BudgetCategoryId = transportCategory.BudgetCategoryId, BudgetSubCategoryId = null });
        }

        if (livingCategory != null)
        {
            await AddExpense(new Expense { ExpenseId = Guid.NewGuid(), ExpenseName = "Groceries", DueDay = 7, AmountDue = 400m, DisplayOrder = order++, BudgetId = budgetId, BudgetCategoryId = livingCategory.BudgetCategoryId, BudgetSubCategoryId = null });
            await AddExpense(new Expense { ExpenseId = Guid.NewGuid(), ExpenseName = "Dining Out", DueDay = 18, AmountDue = 150m, DisplayOrder = order++, BudgetId = budgetId, BudgetCategoryId = livingCategory.BudgetCategoryId, BudgetSubCategoryId = null });
        }

        if (entertainmentCategory != null)
        {
            await AddExpense(new Expense { ExpenseId = Guid.NewGuid(), ExpenseName = "Netflix", DueDay = 10, AmountDue = 15m, DisplayOrder = order++, BudgetId = budgetId, BudgetCategoryId = entertainmentCategory.BudgetCategoryId, BudgetSubCategoryId = null });
            await AddExpense(new Expense { ExpenseId = Guid.NewGuid(), ExpenseName = "Spotify Premium", DueDay = 20, AmountDue = 10m, DisplayOrder = order++, BudgetId = budgetId, BudgetCategoryId = entertainmentCategory.BudgetCategoryId, BudgetSubCategoryId = null });
        }

        if (debtsCategory != null)
        {
            await AddExpense(new Expense { ExpenseId = Guid.NewGuid(), ExpenseName = "Credit Card Payment", DueDay = 15, AmountDue = 50m, DisplayOrder = order++, BudgetId = budgetId, BudgetCategoryId = debtsCategory.BudgetCategoryId, BudgetSubCategoryId = creditCardsSub?.BudgetSubCategoryId });
        }

        if (savingsCategory != null)
        {
            await AddExpense(new Expense { ExpenseId = Guid.NewGuid(), ExpenseName = "Savings Account", DueDay = 1, AmountDue = 250m, DisplayOrder = order++, BudgetId = budgetId, BudgetCategoryId = savingsCategory.BudgetCategoryId, BudgetSubCategoryId = emergencyFundSub?.BudgetSubCategoryId });
        }
    }

    /// <summary>
    /// Creates a sample credit card entry and links to its sample expense.
    /// </summary>
    private async Task CreateSampleCreditAsync(Guid budgetId)
    {
        var creditExpense = await _expenseRepository.GetAll()
            .FirstOrDefaultAsync(e => e.BudgetId == budgetId && e.ExpenseName == "Credit Card Payment")
            ?? throw new KeyNotFoundException("Credit Card Payment expense not found.");

        var credit = new Credit
        {
            CreditId = Guid.NewGuid(),
            CreditName = "Credit Card",
            CreditType = "Credit Card",
            InterestRate = 0.1999m,
            CurrentValue = 1000m,
            EstMonthlyInterest = 16.66m,
            EstYearlyInterest = 199.90m,
            PaymentFrequency = FrequencyEnum.Monthly,
            PaymentAmount = 50m,
            PaymentDay = 15,
            DisplayOrder = 1,
            BudgetId = budgetId,
            ExpenseId = creditExpense.ExpenseId
        };

        await _creditRepository.AddAsync(credit);
        await _creditRepository.SaveChangesAsync();

        creditExpense.CreditId = credit.CreditId;
        await _expenseRepository.UpdateAsync(creditExpense);
        await _expenseRepository.SaveChangesAsync();
    }

    /// <summary>
    /// Creates a sample investment entry and links to its sample expense.
    /// </summary>
    private async Task CreateSampleInvestmentAsync(Guid budgetId)
    {
        var investmentExpense = await _expenseRepository.GetAll()
            .FirstOrDefaultAsync(e => e.BudgetId == budgetId && e.ExpenseName == "Savings Account")
            ?? throw new KeyNotFoundException("Savings Account expense not found.");

        var investment = new Investment
        {
            InvestmentId = Guid.NewGuid(),
            InvestmentName = "Savings Account",
            CurrentValue = 5000m,
            InterestRate = 0.0425m,
            ContributionAmount = 250m,
            ContributionFrequency = FrequencyEnum.Monthly,
            EstYearlyGrowth = 2612.50m,
            DisplayOrder = 1,
            BudgetId = budgetId,
            ExpenseId = investmentExpense.ExpenseId
        };

        await _investmentRepository.AddAsync(investment);
        await _investmentRepository.SaveChangesAsync();

        investmentExpense.InvestmentId = investment.InvestmentId;
        await _expenseRepository.UpdateAsync(investmentExpense);
        await _expenseRepository.SaveChangesAsync();
    }

    #endregion Private Methods for Starter Budget Creation
}
