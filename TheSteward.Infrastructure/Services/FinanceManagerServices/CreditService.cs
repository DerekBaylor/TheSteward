using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TheSteward.Core.IRepositories.FinanceManagerIRepositories;
using TheSteward.Core.IServices.FinanceManagerIServices;
using TheSteward.Core.Models.FinanceManagerModels;
using TheSteward.Core.Dtos.FinanceManagerDtos;

namespace TheSteward.Infrastructure.Services.FinanceManagerServices;

public class CreditService : ICreditService
{
    private readonly ICreditRepository _creditRepository;
    private readonly IMapper _mapper;

    public CreditService(ICreditRepository creditRepository, IMapper mapper) 
    {
        _creditRepository = creditRepository;
        _mapper = mapper;
    }

    public async Task<CreditDto> AddAsync(CreateCreditDto creditDto)
    {
        if (creditDto == null)
            throw new ArgumentNullException(nameof(creditDto));

        var credit = new Credit
        {
            CreditId = Guid.NewGuid(),
            CreditName = creditDto.CreditName,
            CreditType = creditDto.CreditType,
            InterestRate = creditDto.InterestRate,
            CurrentValue = creditDto.CurrentValue,
            EstMonthlyInterest = creditDto.EstMonthlyInterest,
            EstYearlyInterest = creditDto.EstYearlyInterest,
            PaymentFrequency = creditDto.PaymentFrequency,
            PaymentAmount = creditDto.PaymentAmount,
            PaymentDay = creditDto.PaymentDay,
            DisplayOrder = creditDto.DisplayOrder,
            BudgetId = creditDto.BudgetId,
            ExpenseId = creditDto.ExpenseId
        };
        
        await _creditRepository.AddAsync(credit);
        
        return _mapper.Map<CreditDto>(credit);
    }
    
    public async Task<UpdateCreditDto> UpdateAsync(UpdateCreditDto creditDto)
    {
        if (creditDto == null)
            throw new ArgumentNullException(nameof(creditDto));

        var credit = await _creditRepository.GetByIdAsync(creditDto.CreditId);
        if (credit == null)
            throw new KeyNotFoundException($"Credit with ID {creditDto.CreditId} not found.");

        // Update properties
        credit.CreditName = creditDto.CreditName;
        credit.CreditType = creditDto.CreditType;
        credit.InterestRate = creditDto.InterestRate;
        credit.CurrentValue = creditDto.CurrentValue;
        credit.EstMonthlyInterest = creditDto.EstMonthlyInterest;
        credit.EstYearlyInterest = creditDto.EstYearlyInterest;
        credit.PaymentFrequency = creditDto.PaymentFrequency;
        credit.PaymentAmount = creditDto.PaymentAmount;
        credit.PaymentDay = creditDto.PaymentDay;
        credit.DisplayOrder = creditDto.DisplayOrder;
        credit.ExpenseId = creditDto.ExpenseId ?? Guid.Empty;

        await _creditRepository.UpdateAsync(credit);

        return creditDto;
    }

    public async Task DeleteAsync(Guid creditId)
    {
        if (creditId == Guid.Empty)
            throw new ArgumentException("Credit ID cannot be empty.", nameof(creditId));

        var credit = await _creditRepository.GetByIdAsync(creditId);
        if (credit == null)
            throw new KeyNotFoundException($"Credit with ID {creditId} not found.");

        await _creditRepository.DeleteAsync(credit);
    }
    public async Task<CreditDto?> GetAsync(Guid creditId)
    {
        if (creditId == Guid.Empty)
            throw new ArgumentException("Credit ID cannot be empty.", nameof(creditId));

        var credit = await _creditRepository.GetByIdAsync(creditId);

        return credit == null ? null : _mapper.Map<CreditDto>(credit);
    }

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
}