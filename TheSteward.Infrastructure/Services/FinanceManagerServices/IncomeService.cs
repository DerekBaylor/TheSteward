using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TheSteward.Core.Dtos.FinanceManagerDtos;
using TheSteward.Core.IRepositories;
using TheSteward.Core.IRepositories.FinanceManagerIRepositories;
using TheSteward.Core.IServices.FinanceManagerIServices;
using TheSteward.Core.Models.FinanceManagerModels;

namespace TheSteward.Infrastructure.Services.FinanceManagerServices;

public class IncomeService : BaseService<Income>, IIncomeService
{
    private readonly IIncomeRepository _incomeRepository;
    private readonly IMapper _mapper;

    public IncomeService(IBaseRepository<Income> baseRepository, IIncomeRepository incomeRepository, IMapper mapper) :
        base(baseRepository)
    {
        _incomeRepository = incomeRepository;
        _mapper = mapper;
    }

    public async Task<IncomeDto> AddAsync(CreateIncomeDto incomeDto)
    {
        if (incomeDto == null)
            throw new ArgumentNullException(nameof(incomeDto));

        // Create the Income entity
        var income = new Income
        {
            IncomeId = Guid.NewGuid(),
            IncomeName = incomeDto.IncomeName,
            IncomeFrequency = incomeDto.IncomeFrequency,
            PayCheckGross = incomeDto.PayCheckGross,
            BudgetId = incomeDto.BudgetId,
            DisplayOrder = incomeDto.DisplayOrder
        };

        // Calculate derived values
        CalculateIncomeValues(income);

        // Save to database
        await _incomeRepository.AddAsync(income);

        // Map to DTO and return
        return _mapper.Map<IncomeDto>(income);
    }

    public async Task<UpdateIncomeDto> UpdateAsync(UpdateIncomeDto incomeDto)
    {
        if (incomeDto == null)
            throw new ArgumentNullException(nameof(incomeDto));

        // Retrieve existing income
        var income = await GetByIdAsync(incomeDto.IncomeId);
        if (income == null)
            throw new KeyNotFoundException($"Income with ID {incomeDto.IncomeId} not found.");

        // Update properties
        income.IncomeName = incomeDto.IncomeName;
        income.IncomeFrequency = incomeDto.IncomeFrequency;
        income.PayCheckGross = incomeDto.PayCheckGross;
        income.DisplayOrder = incomeDto.DisplayOrder;

        // Recalculate derived values
        CalculateIncomeValues(income);

        // Save changes
        await _incomeRepository.UpdateAsync(income);

        return incomeDto;
    }

    public async Task DeleteAsync(Guid incomeId)
    {
        if (incomeId == Guid.Empty)
            throw new ArgumentException("Income ID cannot be empty.", nameof(incomeId));

        var income = await GetByIdAsync(incomeId);
        if (income == null)
            throw new KeyNotFoundException($"Income with ID {incomeId} not found.");

        await _incomeRepository.DeleteAsync(income);
    }

    public async Task<IncomeDto?> GetAsync(Guid incomeId)
    {
        if (incomeId == Guid.Empty)
            throw new ArgumentException("Income ID cannot be empty.", nameof(incomeId));

        var income = await GetByIdAsync(incomeId);

        return income == null ? null : _mapper.Map<IncomeDto>(income);
    }

    public async Task<List<IncomeDto>> GetAllByBudgetIdAsync(Guid budgetId)
    {
        if (budgetId == Guid.Empty)
            throw new ArgumentException("Budget ID cannot be empty.", nameof(budgetId));

        var incomes = await GetAll()
            .Where(i => i.BudgetId == budgetId)
            .OrderBy(i => i.DisplayOrder)
            .ToListAsync();

        return incomes.Select(i => _mapper.Map<IncomeDto>(i)).ToList();
    }

    #region Private Helper Methods
    
    /// <summary>
    /// Calculates all derived income values including yearly salary, taxes, and monthly net income.
    /// </summary>
    /// <param name="income">The income entity to calculate values for.</param>
    private void CalculateIncomeValues(Income income)
    {
        income.YearlyGrossSalary = income.PayCheckGross * income.IncomeFrequency;
        
        income.EstFederalIncomeTax = CalculateFederalIncomeTax(income.YearlyGrossSalary);
        
        income.EstStateIncomeTax = CalculateStateIncomeTax(income.YearlyGrossSalary);
        
        var yearlyNetIncome = income.YearlyGrossSalary - income.EstFederalIncomeTax - income.EstStateIncomeTax;
        income.MonthlyNetIncome = yearlyNetIncome / 12;
    }

    /// <summary>
    /// Calculates estimated federal income tax based on 2025 tax brackets (single filer, 0 allowances).
    /// </summary>
    /// <param name="yearlyGrossSalary">The gross yearly salary.</param>
    /// <returns>The estimated federal income tax amount.</returns>
    /// <remarks>
    /// This is a simplified calculation. For production, consider using actual tax bracket calculations
    /// with standard deductions and current year tax rates.
    /// </remarks>
    private decimal CalculateFederalIncomeTax(decimal yearlyGrossSalary)
    {
        // 2024 Federal Tax Brackets (Single Filer) - Simplified
        // TODO: Update to take different types of deduction types.
        const decimal standardDeduction = 15750; // 2025 Single Std. Deduction
        var taxableIncome = Math.Max(0, yearlyGrossSalary - standardDeduction);

        decimal tax = 0m;

        if (taxableIncome <= 11600m)
        {
            tax = taxableIncome * 0.10m;
        }
        else if (taxableIncome <= 47150m)
        {
            tax = 1160m + ((taxableIncome - 11600m) * 0.12m);
        }
        else if (taxableIncome <= 100525m)
        {
            tax = 5426m + ((taxableIncome - 47150m) * 0.22m);
        }
        else if (taxableIncome <= 191950m)
        {
            tax = 17168.50m + ((taxableIncome - 100525m) * 0.24m);
        }
        else if (taxableIncome <= 243725m)
        {
            tax = 39110.50m + ((taxableIncome - 191950m) * 0.32m);
        }
        else if (taxableIncome <= 609350m)
        {
            tax = 55678.50m + ((taxableIncome - 243725m) * 0.35m);
        }
        else
        {
            tax = 183647.25m + ((taxableIncome - 609350m) * 0.37m);
        }

        return Math.Round(tax, 2);
    }

    /// <summary>
    /// Calculates estimated state income tax.
    /// </summary>
    /// <param name="yearlyGrossSalary">The gross yearly salary.</param>
    /// <returns>The estimated state income tax amount.</returns>
    /// <remarks>
    /// This is a placeholder implementation. You should implement state-specific tax calculations
    /// based on the user's state of residence. Some states have no income tax.
    /// </remarks>
    private decimal CalculateStateIncomeTax(decimal yearlyGrossSalary)
    {
        // TODO: Implement state-specific tax calculation
        const decimal stateTaxRate = 0.06m;
        return Math.Round(yearlyGrossSalary * stateTaxRate, 2);
    }
    
    #endregion Private Helper Methods
}
