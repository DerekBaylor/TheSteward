using Microsoft.EntityFrameworkCore;
using TheSteward.Core.Dtos.FinanceManagerDtos;
using TheSteward.Core.IRepositories.FinanceManagerIRepositories;
using TheSteward.Core.IServices.FinanceManagerIServices;
using TheSteward.Core.Models.FinanceManagerModels;
using TheSteward.Core.MappingExtensions;

namespace TheSteward.Infrastructure.Services.FinanceManagerServices;

public class InvestmentService : IInvestmentService
{
    private readonly IInvestmentRepository _investmentRepository;

    public InvestmentService(IInvestmentRepository investmentRepository)
    {
        _investmentRepository =  investmentRepository;
    }

    public async Task<InvestmentDto> AddAsync(CreateInvestmentDto investmentDto)
    {
        if (investmentDto == null)
            throw new ArgumentNullException(nameof(investmentDto));

        var investmentId = Guid.NewGuid();
        var investment = investmentDto.ToEntity(investmentId);

        await _investmentRepository.AddAsync(investment);
        await _investmentRepository.SaveChangesAsync();

        return investment.ToDto();
    }


    public async Task<InvestmentDto> UpdateAsync(UpdateInvestmentDto investmentDto)
    {
        if (investmentDto == null)
            throw new ArgumentNullException(nameof(investmentDto));

        var investment = await _investmentRepository.GetByIdAsync(investmentDto.InvestmentId);
        if (investment == null)
            throw new KeyNotFoundException($"Investment with ID {investmentDto.InvestmentId} not found.");

        investment.ApplyUpdate(investmentDto);

        await _investmentRepository.UpdateAsync(investment);
        await _investmentRepository.SaveChangesAsync();

        return investment.ToDto();
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
        return investment?.ToDto();
    }

    public async Task<InvestmentDto?> GetWithExpenseAsync(Guid investmentId)
    {
        if (investmentId == Guid.Empty)
            throw new ArgumentException("Investment ID cannot be empty.", nameof(investmentId));

        var investment = await _investmentRepository.GetAll()
            .Include(i => i.LinkedExpense)
            .FirstOrDefaultAsync(i => i.InvestmentId == investmentId);

        return investment?.ToDto();
    }

    public async Task<List<InvestmentDto>> GetAllByBudgetIdAsync(Guid budgetId)
    {
        if (budgetId == Guid.Empty)
            throw new ArgumentException("Budget ID cannot be empty.", nameof(budgetId));

        var investments = await _investmentRepository.GetAll()
            .Where(i => i.BudgetId == budgetId)
            .OrderBy(i => i.DisplayOrder)
            .ToListAsync();

        return investments.ToDtoList();
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

        return investments.ToDtoList();
    }

    #endregion Get Methods

}