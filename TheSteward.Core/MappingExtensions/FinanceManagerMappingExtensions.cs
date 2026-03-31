using System.Diagnostics.CodeAnalysis;
using TheSteward.Core.Dtos.FinanceManagerDtos;
using TheSteward.Core.Models.FinanceManagerModels;

namespace TheSteward.Core.MappingExtensions;

public static class FinanceManagerMappingExtensions
{
    #region Budget

    /// <summary>
    /// Explicit override required — GenericMapper cannot resolve nested collection ToDto() calls.
    /// All navigation collections must be eagerly loaded before calling this method.
    /// </summary>
    public static BudgetDto ToDto(this Budget src)
    {
        var dto = new BudgetDto
        {
            BudgetId = src.BudgetId,
            BudgetName = src.BudgetName,
            HouseholdId = src.HouseholdId,
            OwnerId = src.OwnerId,
            IsDefaultBudget = src.IsDefaultBudget,
            IsPublic = src.IsPublic,
            CreatedDate = src.CreatedDate,
            ModifiedDate = src.ModifiedDate,

            // Collections — each delegates to its own ToDto() override
            BudgetCategories = src.BudgetCategories is not null
                ? src.BudgetCategories.ToDtoList(includeSubCategories: true)
                : new(),

            Incomes = src.Incomes is not null
                ? src.Incomes.ToDtoList()
                : new(),

            Investments = src.Investments is not null
                ? src.Investments.ToDtoList()   // flattens LinkedExpense via Investment.ToDto()
                : new(),

            Credits = src.Credits is not null
                ? src.Credits.ToDtoList()
                : new(),

            Expenses = src.Expenses is not null
                ? src.Expenses.ToDtoList()      // flattens BudgetCategory, BudgetSubCategory, LinkedCredit, LinkedInvestment
                : new(),
        };

        return dto;
    }

    /// <summary>
    /// Converts a sequence of Budget entities to a list of BudgetDto objects.
    /// </summary>
    /// <param name="src">The sequence of Budget entities to convert. Cannot be null.</param>
    /// <returns>A list of BudgetDto objects representing the converted Budget entities. Returns an empty list if the source
    /// sequence is empty.</returns>
    public static List<BudgetDto> ToDtoList(this IEnumerable<Budget> src)
        => src.Select(b => b.ToDto()).ToList();


    /// <summary>
    /// Creates a new Budget entity from the specified CreateBudgetDto instance.
    /// </summary>
    /// <remarks>The returned Budget entity will have a unique identifier and the current UTC time as its
    /// creation date, regardless of the values in the source object.</remarks>
    /// <param name="src">The source CreateBudgetDto containing the data to initialize the Budget entity. Cannot be null.</param>
    /// <returns>A new Budget entity populated with values from the source. The BudgetId and CreatedDate properties are set
    /// automatically.</returns>
    public static Budget ToEntity(this CreateBudgetDto src, Guid budgetId, DateTime createdDate)
    {
        var entity = GenericMapper.Map<CreateBudgetDto, Budget>(src);
        entity.BudgetId = budgetId;
        entity.CreatedDate = createdDate;

        return entity;
    }

    /// <summary>
    /// Household navigation is intentionally left untouched — EF owns it.
    /// ModifiedDate is stamped here rather than relying on the caller.
    /// </summary>
    public static void ApplyUpdate(this Budget entity, UpdateBudgetDto src, DateTime modifiedDate)
    {
        GenericMapper.MapProperties(src, entity);
        entity.ModifiedDate = modifiedDate;
    }

    #endregion Budget

    #region BudgetCategory

    /// <summary>
    /// SubCategories are opt-in to avoid N+1 surprises.
    /// Pass includeSubCategories: true only when the navigation is eagerly loaded.
    /// </summary>
    public static BudgetCategoryDto ToDto(this BudgetCategory src, bool includeSubCategories = false)
    {
        var dto = GenericMapper.Map<BudgetCategory, BudgetCategoryDto>(src);
        dto.BudgetSubCategories = includeSubCategories && src.BudgetSubCategories is not null
            ? src.BudgetSubCategories.Select(sc => sc.ToDto()).ToList()
            : new();

        return dto;
    }

    /// <summary>
    /// Converts a sequence of BudgetCategory entities to a list of BudgetCategoryDto objects.
    /// </summary>
    /// <param name="src">The sequence of BudgetCategory entities to convert.</param>
    /// <param name="includeSubCategories">true to include subcategories in each BudgetCategoryDto; otherwise, false.</param>
    /// <returns>A list of BudgetCategoryDto objects representing the converted BudgetCategory entities. The list will be empty
    /// if the source sequence contains no elements.</returns>
    public static List<BudgetCategoryDto> ToDtoList(
        this IEnumerable<BudgetCategory> src, bool includeSubCategories = false)
        => src.Select(c => c.ToDto(includeSubCategories)).ToList();

    /// <summary>
    /// Converts a CreateBudgetCategoryDto instance to a BudgetCategory entity.
    /// </summary>
    /// <param name="src">The source CreateBudgetCategoryDto to convert. Cannot be null.</param>
    /// <returns>A BudgetCategory entity populated with values from the source DTO. The BudgetCategoryId property is assigned a
    /// new value if it is empty.</returns>
    public static BudgetCategory ToEntity(this CreateBudgetCategoryDto src, Guid categoryId)
    {
        var entity = GenericMapper.Map<CreateBudgetCategoryDto, BudgetCategory>(src);
        entity.BudgetCategoryId = categoryId;

        return entity;
    }


    /// <summary>
    /// Updates the properties of the specified budget category entity using values from the provided update data
    /// transfer object.
    /// </summary>
    /// <remarks>This method copies values from the update DTO to the entity. Only properties present in the
    /// DTO will be updated. The method does not persist changes to a data store; callers are responsible for saving
    /// changes if needed.</remarks>
    /// <param name="entity">The budget category entity to update. This instance will have its properties set to match those in <paramref
    /// name="src"/>.</param>
    /// <param name="src">The data transfer object containing updated values to apply to the budget category entity. Cannot be null.</param>
    public static void ApplyUpdate(this BudgetCategory entity, UpdateBudgetCategoryDto src)
        => GenericMapper.MapProperties(src, entity);


    #endregion BudgetCategory

    #region BudgetSubCategory

    /// <summary>
    /// Converts a BudgetSubCategory entity to its corresponding BudgetSubCategoryDto representation.
    /// </summary>
    /// <remarks>BudgetSubCategory is fully flat — all fields are mapped directly with no nested navigations.</remarks>
    /// <param name="src">The BudgetSubCategory instance to convert. Cannot be null.</param>
    /// <returns>A BudgetSubCategoryDto object containing the mapped values from the specified BudgetSubCategory instance.</returns>
    public static BudgetSubCategoryDto ToDto(this BudgetSubCategory src)
        => GenericMapper.Map<BudgetSubCategory, BudgetSubCategoryDto>(src);

    /// <summary>
    /// Converts a sequence of BudgetSubCategory entities to a list of BudgetSubCategoryDto objects.
    /// </summary>
    /// <param name="src">The sequence of BudgetSubCategory entities to convert. Cannot be null.</param>
    /// <returns>A list of BudgetSubCategoryDto objects representing the converted entities. Returns an empty list if the
    /// source sequence is empty.</returns>
    public static List<BudgetSubCategoryDto> ToDtoList(this IEnumerable<BudgetSubCategory> src)
        => GenericMapper.MapList<BudgetSubCategory, BudgetSubCategoryDto>(src);

    /// <summary>
    /// Creates a new BudgetSubCategory entity from the specified CreateBudgetSubCategoryDto instance.
    /// </summary>
    /// <param name="src">The source CreateBudgetSubCategoryDto containing the data to initialize the entity. Cannot be null.</param>
    /// <returns>A new BudgetSubCategory entity populated with values from the source DTO. The BudgetSubCategoryId
    /// property is assigned a new value if it is empty.</returns>
    public static BudgetSubCategory ToEntity(this CreateBudgetSubCategoryDto src, Guid subCategoryId)
    {
        var entity = GenericMapper.Map<CreateBudgetSubCategoryDto, BudgetSubCategory>(src);
        entity.BudgetSubCategoryId = subCategoryId;

        return entity;
    }


    /// <summary>
    /// Updates the properties of the specified BudgetSubCategory entity using values from the provided update DTO.
    /// </summary>
    /// <param name="entity">The BudgetSubCategory entity to update. Cannot be null.</param>
    /// <param name="src">The data transfer object containing updated values to apply. Cannot be null.</param>
    public static void ApplyUpdate(this BudgetSubCategory entity, UpdateBudgetSubCategoryDto src)
        => GenericMapper.MapProperties(src, entity);


    #endregion BudgetSubCategory

    #region Credit

    /// <summary>
    /// Converts a Credit entity to its corresponding CreditDto representation.
    /// </summary>
    /// <remarks>Credit and CreditDto are flat scalars — all fields are mapped directly with no nested navigations.</remarks>
    /// <param name="src">The Credit instance to convert. Cannot be null.</param>
    /// <returns>A CreditDto object containing the mapped values from the specified Credit instance.</returns>
    public static CreditDto ToDto(this Credit src)
        => GenericMapper.Map<Credit, CreditDto>(src);

    /// <summary>
    /// Converts a sequence of Credit entities to a list of CreditDto objects.
    /// </summary>
    /// <param name="src">The sequence of Credit entities to convert. Cannot be null.</param>
    /// <returns>A list of CreditDto objects representing the converted entities. Returns an empty list if the source
    /// sequence is empty.</returns>
    public static List<CreditDto> ToDtoList(this IEnumerable<Credit> src)
        => GenericMapper.MapList<Credit, CreditDto>(src);

    /// <summary>
    /// Creates a new Credit entity from the specified CreateCreditDto instance.
    /// </summary>
    /// <remarks>A new CreditId is always generated. If the source DTO does not supply an ExpenseId,
    /// ExpenseId is set to <see cref="Guid.Empty"/> as a sentinel value rather than left uninitialized.</remarks>
    /// <param name="src">The source CreateCreditDto containing the data to initialize the entity. Cannot be null.</param>
    /// <returns>A new Credit entity populated with values from the source DTO.</returns>
    public static Credit ToEntity(this CreateCreditDto src)
    {
        var entity = GenericMapper.Map<CreateCreditDto, Credit>(src);
        entity.ExpenseId = src.ExpenseId ?? Guid.Empty;

        return entity;
    }

    /// <summary>
    /// Converts a CreateCreditDto instance to a Credit entity, assigning the specified credit and expense identifiers.
    /// </summary>
    /// <param name="src">The source CreateCreditDto object to convert. Cannot be null.</param>
    /// <param name="creditI">The unique identifier to assign to the Credit entity's CreditId property.</param>
    /// <param name="expenseId">The unique identifier to assign to the Credit entity's ExpenseId property.</param>
    /// <returns>A Credit entity populated with values from the source DTO and the specified identifiers.</returns>
    public static Credit ToEntity(this CreateCreditDto src, Guid creditI, Guid expenseId)
    {
        var entity = GenericMapper.Map<CreateCreditDto, Credit>(src);
        entity.CreditId = creditI;
        entity.ExpenseId = expenseId;
        
        return entity;
    }

    /// <summary>
    /// Updates the properties of the specified Credit entity using values from the provided update DTO.
    /// </summary>
    /// <remarks>ExpenseId is only overwritten when the caller explicitly supplies a value on the DTO.
    /// Passing a null ExpenseId leaves the existing entity value unchanged.</remarks>
    /// <param name="entity">The Credit entity to update. Cannot be null.</param>
    /// <param name="src">The data transfer object containing updated values to apply. Cannot be null.</param>
    public static void ApplyUpdate(this Credit entity, UpdateCreditDto src)
    {
        entity.CreditName = src.CreditName;
        entity.CreditType = src.CreditType;
        entity.CurrentValue = src.CurrentValue;
        entity.InterestRate = src.InterestRate;
        entity.PaymentAmount = src.PaymentAmount;
        entity.PaymentFrequency = src.PaymentFrequency;
        entity.PaymentDay = src.PaymentDay;
        entity.EstMonthlyInterest = src.EstMonthlyInterest;
        entity.EstYearlyInterest = src.EstYearlyInterest;
        entity.DisplayOrder = src.DisplayOrder;

        // Only update BudgetId if a valid one is provided
        if (src.BudgetId != Guid.Empty)
            entity.BudgetId = src.BudgetId;

        // Only update ExpenseId if a valid one is explicitly provided
        if (src.ExpenseId.HasValue && src.ExpenseId.Value != Guid.Empty)
            entity.ExpenseId = src.ExpenseId.Value;
    }




    #endregion Credit

    #region Expense

    /// <summary>
    /// Navigation properties are flattened from loaded navigations.
    /// LinkedInvestmentDto and LinkedCreditDto are intentionally NOT recursively 
    /// mapped to prevent circular reference loops. Use the FK IDs for lookups instead.
    /// </summary>
    public static ExpenseDto ToDto(this Expense src)
    {
        var dto = GenericMapper.Map<Expense, ExpenseDto>(src);

        dto.BudgetCategory = src.BudgetCategory?.ToDto()
            ?? new BudgetCategoryDto { BudgetCategoryName = string.Empty };

        dto.BudgetSubCategory = src.BudgetSubCategory?.ToDto()
            ?? new BudgetSubCategoryDto { BudgetSubCategoryName = string.Empty };

        // These caused the circular reference — set to null, use FK IDs instead
        dto.LinkedCreditDto = null;
        dto.LinkedInvestmentDto = null;

        return dto;
    }


    /// <summary>
    /// Converts a sequence of Expense entities to a list of ExpenseDto objects.
    /// </summary>
    /// <remarks>Each Expense is converted via <see cref="ToDto(Expense)"/>, which flattens all loaded
    /// navigation properties. Ensure navigations are eagerly loaded before calling this method to avoid
    /// empty nested DTOs.</remarks>
    /// <param name="src">The sequence of Expense entities to convert. Cannot be null.</param>
    /// <returns>A list of ExpenseDto objects representing the converted entities. Returns an empty list if the source
    /// sequence is empty.</returns>
    public static List<ExpenseDto> ToDtoList(this IEnumerable<Expense> src)
        => src.Select(e => e.ToDto()).ToList();

    /// <summary>
    /// Creates a new Expense entity from the specified CreateExpenseDto instance.
    /// </summary>
    /// <remarks>Navigation properties are intentionally omitted — Entity Framework resolves them
    /// via the mapped foreign keys.</remarks>
    /// <param name="src">The source CreateExpenseDto containing the data to initialize the entity. Cannot be null.</param>
    /// <returns>A new Expense entity populated with scalar values from the source DTO. A new ExpenseId is assigned
    /// automatically.</returns>
    public static Expense ToEntity(this CreateExpenseDto src, Guid expenseId)
    {
        var entity = GenericMapper.Map<CreateExpenseDto, Expense>(src);
        entity.ExpenseId = expenseId;

        return entity;
    }


    /// <summary>
    /// Updates the scalar properties of the specified Expense entity using values from the provided update DTO.
    /// </summary>
    /// <remarks>Navigation properties are intentionally omitted — Entity Framework resolves them
    /// via the mapped foreign keys.</remarks>
    /// <param name="entity">The Expense entity to update. Cannot be null.</param>
    /// <param name="src">The data transfer object containing updated values to apply. Cannot be null.</param>
    //public static void ApplyUpdate(this Expense entity, UpdateExpenseDto src)
    //    => GenericMapper.MapProperties(src, entity);

    public static void ApplyUpdate(this Expense entity, UpdateExpenseDto src)
    {
        entity.ExpenseName = src.ExpenseName;
        entity.AmountDue = src.AmountDue;
        entity.DueDay = src.DueDay;
        entity.DisplayOrder = src.DisplayOrder;
        entity.BudgetCategoryId = src.BudgetCategoryId;
        entity.BudgetSubCategoryId = src.BudgetSubCategoryId;
        entity.CreditId = src.CreditId;
        entity.InvestmentId = src.InvestmentId;

        if (src.BudgetId != Guid.Empty)
            entity.BudgetId = src.BudgetId;
    }


    #endregion

        #region Income

        /// <summary>
        /// Converts an Income entity to its corresponding IncomeDto representation.
        /// </summary>
        /// <param name="src">The Income instance to convert. Cannot be null.</param>
        /// <returns>An IncomeDto object containing the mapped values from the specified Income instance.</returns>
    public static IncomeDto ToDto(this Income src)
        => GenericMapper.Map<Income, IncomeDto>(src);

    /// <summary>
    /// Converts a sequence of Income entities to a list of IncomeDto objects.
    /// </summary>
    /// <param name="src">The sequence of Income entities to convert. Cannot be null.</param>
    /// <returns>A list of IncomeDto objects representing the converted entities. Returns an empty list if the source
    /// sequence is empty.</returns>
    public static List<IncomeDto> ToDtoList(this IEnumerable<Income> src)
        => GenericMapper.MapList<Income, IncomeDto>(src);

    public static Income ToEntity(this CreateIncomeDto src, Guid incomeId)
    {
        var entity = GenericMapper.Map<CreateIncomeDto, Income>(src);
        entity.IncomeId = incomeId;

        return entity;
    }

    public static void ApplyUpdate(this Income entity, UpdateIncomeDto src)
        => GenericMapper.MapProperties(src, entity);

    #endregion Income

    #region Investment

    /// <summary>
    /// LinkedExpense is intentionally NOT mapped here to prevent circular reference:
    /// Expense → LinkedInvestment → LinkedExpense → LinkedInvestment → ...
    /// The ExpenseId FK on InvestmentDto is sufficient for lookups.
    /// </summary>
    public static InvestmentDto ToDto(this Investment src)
    {
        var dto = GenericMapper.Map<Investment, InvestmentDto>(src);
        dto.LinkedExpenseDto = null;
        return dto;
    }


    /// <summary>
    /// Converts a sequence of Investment entities to a list of InvestmentDto objects.
    /// </summary>
    /// <remarks>Each Investment is converted via <see cref="ToDto(Investment)"/>, which flattens the
    /// LinkedExpense navigation if loaded. Ensure the navigation is eagerly loaded before calling this
    /// method to avoid null LinkedExpenseDto values.</remarks>
    /// <param name="src">The sequence of Investment entities to convert. Cannot be null.</param>
    /// <returns>A list of InvestmentDto objects representing the converted entities. Returns an empty list if the
    /// source sequence is empty.</returns>
    public static List<InvestmentDto> ToDtoList(this IEnumerable<Investment> src)
        => src.Select(i => i.ToDto()).ToList();

    /// <summary>
    /// Creates a new Investment entity from the specified CreateInvestmentDto instance.
    /// </summary>
    /// <remarks>Budget and LinkedExpense navigation properties are intentionally omitted — Entity Framework
    /// resolves them via the mapped foreign keys.</remarks>
    /// <param name="src">The source CreateInvestmentDto containing the data to initialize the entity. Cannot be null.</param>
    /// <returns>A new Investment entity populated with scalar values from the source DTO. A new InvestmentId is
    /// assigned automatically.</returns>
    public static Investment ToEntity(this CreateInvestmentDto src, Guid investmentId)
    {
        var entity = GenericMapper.Map<CreateInvestmentDto, Investment>(src);
        entity.InvestmentId = investmentId;

        return entity;
    }


    /// <summary>
    /// Updates the scalar properties of the specified Investment entity using values from the provided update DTO.
    /// </summary>
    /// <remarks>Budget and LinkedExpense navigation properties are intentionally omitted — Entity Framework
    /// resolves them via the mapped foreign keys.</remarks>
    /// <param name="entity">The Investment entity to update. Cannot be null.</param>
    /// <param name="src">The data transfer object containing updated values to apply. Cannot be null.</param>
    //public static void ApplyUpdate(this Investment entity, UpdateInvestmentDto src)
    //    => GenericMapper.MapProperties(src, entity);

    public static void ApplyUpdate(this Investment investment, UpdateInvestmentDto dto)
    {
        investment.InvestmentName = dto.InvestmentName;
        investment.CurrentValue = dto.CurrentValue;
        investment.InterestRate = dto.InterestRate;
        investment.ContributionAmount = dto.ContributionAmount;
        investment.ContributionFrequency = dto.ContributionFrequency;
        investment.EstYearlyGrowth = dto.EstYearlyGrowth;
        investment.DisplayOrder = dto.DisplayOrder;
        investment.ExpenseId = dto.ExpenseId;

        if (dto.BudgetId != Guid.Empty)
            investment.BudgetId = dto.BudgetId;

    }

    #endregion Investment
}