//01/20/26
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TheSteward.Core.Dtos.FinanceManagerDtos;
using TheSteward.Core.IRepositories;
using TheSteward.Core.IRepositories.FinanceManagerIRepositories;
using TheSteward.Core.IServices.FinanceManagerIServices;
using TheSteward.Core.Models.FinanceManagerModels;

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

    public async Task<BudgetDto> AddStarterBudget(Guid userHouseholdId)
    {
        if (userHouseholdId == Guid.Empty)
            throw new ArgumentException("Household ID cannot be empty.", nameof(userHouseholdId));
        
        var budget = new Budget
        {
            BudgetId = Guid.NewGuid(),
            BudgetName = "My First Budget",
            HouseholdId = userHouseholdId,
            OwnerId = string.Empty,
            IsDefaultBudget = true,
            IsPublic = false,
            CreatedDate = DateTime.UtcNow
        };

        await _budgetRepository.AddAsync(budget);
        
        await CreateStandardCategoriesAsync(budget.BudgetId);
        
        await CreateSampleIncomeAsync(budget.BudgetId);
        
        await CreateSampleCreditAsync(budget.BudgetId);
        
        await CreateSampleExpensesAsync(budget.BudgetId);
        
        await CreateSampleInvestmentAsync(budget.BudgetId);

        await _budgetRepository.SaveChangesAsync();

        // Reload budget with all related data
        return await GetByIdAsync(budget.BudgetId);
    }

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

    /// <summary>
    /// Creates standard budget categories and subcategories.
    /// </summary>
    private async Task CreateStandardCategoriesAsync(Guid budgetId)
    {
        var categories = new[]
        {
            ("Housing", new[] { "House/Apt" }),
            ("Utilities", Array.Empty<string>()),
            ("Transportation", new[] { "Car One" }),
            ("Living Expenses", new[] { "Food", "Personal Care" }),
            ("Entertainment", new[] { "Streaming Services", "Hobbies" }),
            ("Pets", Array.Empty<string>()),
            ("Health", Array.Empty<string>()),
            ("Savings", new[] { "Emergency Fund", "Retirement", "Investments" }),
            ("Debts", new[] { "Credit Cards", "Student Loans" }),
            ("Child", Array.Empty<string>()),
            ("Other", Array.Empty<string>()),
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

            // Only create subcategories if there are names provided
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
            IncomeFrequency = 26, // Bi-weekly
            PayCheckGross = 2000m,
            YearlyGrossSalary = 52000m,
            EstFederalIncomeTax = 6240m,
            EstStateIncomeTax = 2600m,
            MonthlyNetIncome = 3580m,
            DisplayOrder = 1,
            BudgetId = budgetId
        };

        await _incomeRepository.AddAsync(income);
    }

    /// <summary>
    /// Creates a sample credit card entry.
    /// </summary>
    private async Task CreateSampleCreditAsync(Guid budgetId)
    {
        var credit = new Credit
        {
            CreditId = Guid.NewGuid(),
            CreditName = "Credit Card",
            CreditType = "Credit Card",
            InterestRate = 0.1999m, // 19.99%
            CurrentValue = 1000m,
            EstMonthlyInterest = 16.66m,
            EstYearlyInterest = 199.90m,
            PaymentFrequency = 12, // Monthly
            PaymentAmount = 50m,
            PaymentDay = 15,
            DisplayOrder = 1,
            BudgetId = budgetId,
            ExpenseId = Guid.Empty //TODO: Link to a Credit Expense
        };

        await _creditRepository.AddAsync(credit);
    }

    /// <summary>
    /// Creates sample expense entries.
    /// </summary>
    private async Task CreateSampleExpensesAsync(Guid budgetId)
    {
        // Fetch all categories and subcategories for the budget
        var allCategories = await _categoryRepository.GetAll()
            .Where(c => c.BudgetId == budgetId)
            .Include(c => c.BudgetSubCategories)
            .ToListAsync();

        // Helper to find a category by name
        BudgetCategory? GetCategory(string name) => allCategories.FirstOrDefault(c => c.BudgetCategoryName == name);
        // Helper to find a subcategory by category name and subcategory name
        BudgetSubCategory? GetSubCategory(string categoryName, string subCategoryName) =>
            GetCategory(categoryName)?.BudgetSubCategories.FirstOrDefault(sc => sc.BudgetSubCategoryName == subCategoryName);

        var expensesToAdd = new List<Expense>();
        int displayOrderCounter = 1;

        // --- Housing Expenses ---
        var housingCategory = GetCategory("Housing");
        var houseAptSubCategory = GetSubCategory("Housing", "House/Apt");
        if (housingCategory != null)
        {
            expensesToAdd.Add(new Expense
            {
                ExpenseId = Guid.NewGuid(),
                ExpenseName = "Rent",
                DueDay = 1,
                AmountDue = 1200m,
                DisplayOrder = displayOrderCounter++,
                BudgetId = budgetId,
                BudgetCategoryId = housingCategory.BudgetCategoryId,
                BudgetSubCategoryId = houseAptSubCategory?.BudgetSubCategoryId
            });
            expensesToAdd.Add(new Expense
            {
                ExpenseId = Guid.NewGuid(),
                ExpenseName = "Renter's Insurance",
                DueDay = 1,
                AmountDue = 15m,
                DisplayOrder = displayOrderCounter++,
                BudgetId = budgetId,
                BudgetCategoryId = housingCategory.BudgetCategoryId,
                BudgetSubCategoryId = houseAptSubCategory?.BudgetSubCategoryId
            });
        }

        // --- Utilities Expenses ---
        var utilitiesCategory = GetCategory("Utilities");
        if (utilitiesCategory != null)
        {
            expensesToAdd.Add(new Expense
            {
                ExpenseId = Guid.NewGuid(),
                ExpenseName = "Electric Bill",
                DueDay = 15,
                AmountDue = 100m,
                DisplayOrder = displayOrderCounter++,
                BudgetId = budgetId,
                BudgetCategoryId = utilitiesCategory.BudgetCategoryId,
                BudgetSubCategoryId = null
            });
            expensesToAdd.Add(new Expense
            {
                ExpenseId = Guid.NewGuid(),
                ExpenseName = "Internet Service",
                DueDay = 20,
                AmountDue = 70m,
                DisplayOrder = displayOrderCounter++,
                BudgetId = budgetId,
                BudgetCategoryId = utilitiesCategory.BudgetCategoryId,
                BudgetSubCategoryId = null
            });
        }

        // --- Transportation Expenses ---
        var transportationCategory = GetCategory("Transportation");
        var carOneSubCategory = GetSubCategory("Transportation", "Car One");
        if (transportationCategory != null)
        {
            expensesToAdd.Add(new Expense
            {
                ExpenseId = Guid.NewGuid(),
                ExpenseName = "Car Payment",
                DueDay = 5,
                AmountDue = 350m,
                DisplayOrder = displayOrderCounter++,
                BudgetId = budgetId,
                BudgetCategoryId = transportationCategory.BudgetCategoryId,
                BudgetSubCategoryId = carOneSubCategory?.BudgetSubCategoryId
            });
            expensesToAdd.Add(new Expense
            {
                ExpenseId = Guid.NewGuid(),
                ExpenseName = "Car Insurance",
                DueDay = 10,
                AmountDue = 120m,
                DisplayOrder = displayOrderCounter++,
                BudgetId = budgetId,
                BudgetCategoryId = transportationCategory.BudgetCategoryId,
                BudgetSubCategoryId = carOneSubCategory?.BudgetSubCategoryId
            });
            expensesToAdd.Add(new Expense
            {
                ExpenseId = Guid.NewGuid(),
                ExpenseName = "Gasoline",
                DueDay = 25,
                AmountDue = 150m,
                DisplayOrder = displayOrderCounter++,
                BudgetId = budgetId,
                BudgetCategoryId = transportationCategory.BudgetCategoryId,
                BudgetSubCategoryId = null // No subcategory for this
            });
        }

        // --- Living Expenses ---
        var livingCategory = GetCategory("Living Expenses");
        if (livingCategory != null)
        {
            expensesToAdd.Add(new Expense
            {
                ExpenseId = Guid.NewGuid(),
                ExpenseName = "Groceries",
                DueDay = 7,
                AmountDue = 400m,
                DisplayOrder = displayOrderCounter++,
                BudgetId = budgetId,
                BudgetCategoryId = livingCategory.BudgetCategoryId,
                BudgetSubCategoryId = null // No subcategory for this
            });
            expensesToAdd.Add(new Expense
            {
                ExpenseId = Guid.NewGuid(),
                ExpenseName = "Dining Out",
                DueDay = 18,
                AmountDue = 150m,
                DisplayOrder = displayOrderCounter++,
                BudgetId = budgetId,
                BudgetCategoryId = livingCategory.BudgetCategoryId,
                BudgetSubCategoryId = null // No subcategory for this
            });
        }

        // --- Entertainment Expenses ---
        var entertainmentCategory = GetCategory("Entertainment");
        if (entertainmentCategory != null)
        {
            expensesToAdd.Add(new Expense
            {
                ExpenseId = Guid.NewGuid(),
                ExpenseName = "Netflix",
                DueDay = 10,
                AmountDue = 15m,
                DisplayOrder = displayOrderCounter++,
                BudgetId = budgetId,
                BudgetCategoryId = entertainmentCategory.BudgetCategoryId,
                BudgetSubCategoryId = null // No subcategory for this
            });
            expensesToAdd.Add(new Expense
            {
                ExpenseId = Guid.NewGuid(),
                ExpenseName = "Spotify Premium",
                DueDay = 20,
                AmountDue = 10m,
                DisplayOrder = displayOrderCounter++,
                BudgetId = budgetId,
                BudgetCategoryId = entertainmentCategory.BudgetCategoryId,
                BudgetSubCategoryId = null // No subcategory for this
            });
        }

        // --- Debts Expenses (Credit Card linked to the sample credit) ---
        var debtsCategory = GetCategory("Debts");
        var creditCardsSubCategory = GetSubCategory("Debts", "Credit Cards"); // This subcategory exists
        var sampleCredit = await _creditRepository.GetAll()
            .FirstOrDefaultAsync(c => c.BudgetId == budgetId && c.CreditName == "Credit Cards");
        
        if (debtsCategory != null && creditCardsSubCategory != null && sampleCredit != null)
        {
            var creditCardPaymentExpense = new Expense
            {
                ExpenseId = Guid.NewGuid(),
                ExpenseName = "Credit Card Payment",
                DueDay = sampleCredit.PaymentDay,
                AmountDue = sampleCredit.PaymentAmount,
                DisplayOrder = displayOrderCounter++,
                BudgetId = budgetId,
                BudgetCategoryId = debtsCategory.BudgetCategoryId,
                BudgetSubCategoryId = creditCardsSubCategory.BudgetSubCategoryId,
                CreditId = sampleCredit.CreditId // Link to the credit
            };
            expensesToAdd.Add(creditCardPaymentExpense);
            
            sampleCredit.ExpenseId = creditCardPaymentExpense.ExpenseId;
            await _creditRepository.UpdateAsync(sampleCredit);
        }

        // --- Savings Expenses (linking to Investment for "Debt Payment" subcategory) ---
        var savingsCategory = GetCategory("Savings");
        var debtPaymentSubCategory = GetSubCategory("Savings", "Debt Payment"); // This subcategory exists
        if (savingsCategory != null && debtPaymentSubCategory != null)
        {
             expensesToAdd.Add(new Expense
            {
                ExpenseId = Guid.NewGuid(),
                ExpenseName = "Student Loan Payment",
                DueDay = 28,
                AmountDue = 250m,
                DisplayOrder = displayOrderCounter++,
                BudgetId = budgetId,
                BudgetCategoryId = savingsCategory.BudgetCategoryId,
                BudgetSubCategoryId = debtPaymentSubCategory.BudgetSubCategoryId
            });
        }
        
        // Add all collected expenses
        foreach (var expense in expensesToAdd)
        {
            await _expenseRepository.AddAsync(expense);
        }
    }
    
    /// <summary>
    /// Creates a sample savings account investment entry.
    /// </summary>
    private async Task CreateSampleInvestmentAsync(Guid budgetId)
    {
        var investment = new Investment
        {
            InvestmentId = Guid.NewGuid(),
            InvestmentName = "Savings Account",
            CurrentValue = 5000m, // Current balance
            InterestRate = 0.0425m, // 4.25% APY (typical high-yield savings rate)
            ContributionAmount = 200m, // Monthly contribution
            ContributionFrequency = 12, // Monthly
            EstYearlyGrowth = 2612.50m, // Placeholder - should be calculated on frontend
            DisplayOrder = 1,
            BudgetId = budgetId,
            ExpenseId = Guid.Empty, // Not linked to an expense initially
        };

        await _investmentRepository.AddAsync(investment);
    }

    #endregion
}
