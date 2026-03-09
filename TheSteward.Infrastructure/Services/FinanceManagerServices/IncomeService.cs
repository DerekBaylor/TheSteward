using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TheSteward.Core.Dtos.FinanceManagerDtos;
using TheSteward.Core.IRepositories.FinanceManagerIRepositories;
using TheSteward.Core.IServices.FinanceManagerIServices;
using TheSteward.Core.Models.FinanceManagerModels;

namespace TheSteward.Infrastructure.Services.FinanceManagerServices;

public class IncomeService : IIncomeService
{
    private readonly IIncomeRepository _incomeRepository;
    private readonly IMapper _mapper;

    public IncomeService(IIncomeRepository incomeRepository, IMapper mapper)
    {
        _incomeRepository = incomeRepository;
        _mapper = mapper;
    }

    public async Task<IncomeDto> AddAsync(CreateIncomeDto incomeDto)
    {
        if (incomeDto == null)
            throw new ArgumentNullException(nameof(incomeDto));

        var income = new Income
        {
            IncomeId = Guid.NewGuid(),
            IncomeName = incomeDto.IncomeName,
            IncomeFrequency = incomeDto.IncomeFrequency,
            PayCheckGross = incomeDto.PayCheckGross,
            BudgetId = incomeDto.BudgetId,
            DisplayOrder = incomeDto.DisplayOrder
        };

        CalculateIncomeValues(income);

        await _incomeRepository.AddAsync(income);

        return _mapper.Map<IncomeDto>(income);
    }

    public async Task<UpdateIncomeDto> UpdateAsync(UpdateIncomeDto incomeDto)
    {
        if (incomeDto == null)
            throw new ArgumentNullException(nameof(incomeDto));

        var income = await _incomeRepository.GetByIdAsync(incomeDto.IncomeId);
        if (income == null)
            throw new KeyNotFoundException($"Income with ID {incomeDto.IncomeId} not found.");

        income.IncomeName = incomeDto.IncomeName;
        income.IncomeFrequency = incomeDto.IncomeFrequency;
        income.PayCheckGross = incomeDto.PayCheckGross;
        income.DisplayOrder = incomeDto.DisplayOrder;

        CalculateIncomeValues(income);

        await _incomeRepository.UpdateAsync(income);

        return incomeDto;
    }

    public async Task DeleteAsync(Guid incomeId)
    {
        if (incomeId == Guid.Empty)
            throw new ArgumentException("Income ID cannot be empty.", nameof(incomeId));

        var income = await _incomeRepository.GetByIdAsync(incomeId);
        if (income == null)
            throw new KeyNotFoundException($"Income with ID {incomeId} not found.");

        await _incomeRepository.DeleteAsync(income);
    }

    #region Get Methods
    public async Task<IncomeDto?> GetAsync(Guid incomeId)
    {
        if (incomeId == Guid.Empty)
            throw new ArgumentException("Income ID cannot be empty.", nameof(incomeId));

        var income = await _incomeRepository.GetByIdAsync(incomeId);
        return income == null ? null : _mapper.Map<IncomeDto>(income);
    }

    public async Task<List<IncomeDto>> GetAllByBudgetIdAsync(Guid budgetId)
    {
        if (budgetId == Guid.Empty)
            throw new ArgumentException("Budget ID cannot be empty.", nameof(budgetId));

        var incomes = await _incomeRepository.GetAll()
            .Where(i => i.BudgetId == budgetId)
            .OrderBy(i => i.DisplayOrder)
            .ToListAsync();

        return incomes.Select(i => _mapper.Map<IncomeDto>(i)).ToList();
    }
    #endregion Get Methods

    #region Private Helper Methods

    /// <summary>
    /// Calculates all derived values for a single income: yearly gross,
    /// estimated federal and state taxes, and monthly net.
    /// Note: taxes are estimated per-income in isolation. If a user has
    /// multiple income sources, the actual combined tax burden may be higher
    /// due to bracket progression — the client-side preview communicates this.
    /// </summary>
    private static void CalculateIncomeValues(Income income)
    {
        income.YearlyGrossSalary = income.PayCheckGross * income.IncomeFrequency;
        income.EstFederalIncomeTax = CalculateFederalIncomeTax(income.YearlyGrossSalary);
        income.EstStateIncomeTax = CalculateStateIncomeTax(income.YearlyGrossSalary);

        var yearlyNet = income.YearlyGrossSalary
                                  - income.EstFederalIncomeTax
                                  - income.EstStateIncomeTax;
        income.MonthlyNetIncome = Math.Round(yearlyNet / 12m, 2);
    }

    /// <summary>
    /// 2025 federal tax brackets, single filer, $15,750 standard deduction.
    /// </summary>
    private static decimal CalculateFederalIncomeTax(decimal yearlyGrossSalary)
    {
        const decimal standardDeduction = 15750m;
        var taxable = Math.Max(0, yearlyGrossSalary - standardDeduction);

        return taxable switch
        {
            <= 11600m => Math.Round(taxable * 0.10m, 2),
            <= 47150m => Math.Round(1160m + (taxable - 11600m) * 0.12m, 2),
            <= 100525m => Math.Round(5426m + (taxable - 47150m) * 0.22m, 2),
            <= 191950m => Math.Round(17168.50m + (taxable - 100525m) * 0.24m, 2),
            <= 243725m => Math.Round(39110.50m + (taxable - 191950m) * 0.32m, 2),
            <= 609350m => Math.Round(55678.50m + (taxable - 243725m) * 0.35m, 2),
            _ => Math.Round(183647.25m + (taxable - 609350m) * 0.37m, 2),
        };
    }

    /// <summary>
    /// Placeholder state tax at 6%. Replace with state-specific logic as needed.
    /// </summary>
    private static decimal CalculateStateIncomeTax(decimal yearlyGrossSalary)
    {
        const decimal stateTaxRate = 0.06m;
        return Math.Round(yearlyGrossSalary * stateTaxRate, 2);
    }

    #endregion
}
