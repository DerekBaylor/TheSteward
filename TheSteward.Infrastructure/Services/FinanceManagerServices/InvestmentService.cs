using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TheSteward.Core.Dtos.FinanceManagerDtos;
using TheSteward.Core.IRepositories.FinanceManagerIRepositories;
using TheSteward.Core.IServices.FinanceManagerIServices;
using TheSteward.Core.Models.FinanceManagerModels;

namespace TheSteward.Infrastructure.Services.FinanceManagerServices;

public class InvestmentService : IInvestmentService
{
    private readonly IInvestmentRepository _investmentRepository;
    private readonly IMapper _mapper;

    public InvestmentService(IInvestmentRepository investmentRepository, IMapper mapper)
    {
        _investmentRepository =  investmentRepository;
        _mapper =  mapper;
    }

    public async Task<InvestmentDto> AddAsync(CreateInvestmentDto investmentDto)
    {
        if (investmentDto == null)
            throw new ArgumentNullException(nameof(investmentDto));

        var investment = new Investment
        {
            InvestmentId = Guid.NewGuid(),
            InvestmentName = investmentDto.InvestmentName,
            CurrentValue = investmentDto.CurrentValue,
            InterestRate = investmentDto.InterestRate,
            ContributionAmount = investmentDto.ContributionAmount,
            ContributionFrequency = investmentDto.ContributionFrequency,
            EstYearlyGrowth = investmentDto.EstYearlyGrowth,
            DisplayOrder = investmentDto.DisplayOrder,
            BudgetId = investmentDto.BudgetId,
            ExpenseId = investmentDto.ExpenseId
        };
        
        await _investmentRepository.AddAsync(investment);
        await _investmentRepository.SaveChangesAsync();
        
        return _mapper.Map<InvestmentDto>(investment);
    }

    public async Task<UpdateInvestmentDto> UpdateAsync(UpdateInvestmentDto investmentDto)
    {
        if (investmentDto == null)
            throw new ArgumentNullException(nameof(investmentDto));

        var investment = await _investmentRepository.GetByIdAsync(investmentDto.InvestmentId);
        if (investment == null)
            throw new KeyNotFoundException($"Investment with ID {investmentDto.InvestmentId} not found.");
        
        investment.InvestmentName = investmentDto.InvestmentName;
        investment.CurrentValue = investmentDto.CurrentValue;
        investment.InterestRate = investmentDto.InterestRate;
        investment.ContributionAmount = investmentDto.ContributionAmount;
        investment.ContributionFrequency = investmentDto.ContributionFrequency;
        investment.EstYearlyGrowth = investmentDto.EstYearlyGrowth;
        investment.DisplayOrder = investmentDto.DisplayOrder;
        investment.ExpenseId = investmentDto.ExpenseId;

        await _investmentRepository.UpdateAsync(investment);
        await _investmentRepository.SaveChangesAsync();

        return investmentDto;
    }

    public async Task DeleteAsync(Guid investmentId)
    {
        if (investmentId == Guid.Empty)
            throw new ArgumentException("Investment ID cannot be empty.", nameof(investmentId));

        var investment = await _investmentRepository.GetByIdAsync(investmentId);
        if (investment == null)
            throw new KeyNotFoundException($"Investment with ID {investmentId} not found.");

        await _investmentRepository.DeleteAsync(investment);
        await _investmentRepository.SaveChangesAsync();
    }

    #region Get Methods
    public async Task<InvestmentDto?> GetAsync(Guid investmentId)
    {
        if (investmentId == Guid.Empty)
            throw new ArgumentException("Investment ID cannot be empty.", nameof(investmentId));

        var investment = await _investmentRepository.GetByIdAsync(investmentId);

        return investment == null ? null : _mapper.Map<InvestmentDto>(investment);
    }

    public async Task<InvestmentDto?> GetWithExpenseAsync(Guid investmentId)
    {
        if (investmentId == Guid.Empty)
            throw new ArgumentException("Investment ID cannot be empty.", nameof(investmentId));

        var investment = await _investmentRepository.GetAll()
            .Include(i => i.LinkedExpense)
            .FirstOrDefaultAsync(i => i.InvestmentId == investmentId);

        return investment == null ? null : _mapper.Map<InvestmentDto>(investment);
    }

    public async Task<List<InvestmentDto>> GetAllByBudgetIdAsync(Guid budgetId)
    {
        if (budgetId == Guid.Empty)
            throw new ArgumentException("Budget ID cannot be empty.", nameof(budgetId));

        var investments = await _investmentRepository.GetAll()
            .Where(i => i.BudgetId == budgetId)
            .OrderBy(i => i.DisplayOrder)
            .ToListAsync();

        return _mapper.Map<List<InvestmentDto>>(investments);
    }

    public async Task<List<InvestmentDto>> GetAllByBudgetIdWithExpensesAsync(Guid budgetId)
    {
        if (budgetId == Guid.Empty)
            throw new ArgumentException("Budget ID cannot be empty.", nameof(budgetId));

        var investments = await _investmentRepository.GetAll()
            .Where(i => i.BudgetId == budgetId)
            .Include(i => i.LinkedExpense)
            .OrderBy(i => i.DisplayOrder)
            .ToListAsync();

        return _mapper.Map<List<InvestmentDto>>(investments);
    }

    #endregion Get Methods
}