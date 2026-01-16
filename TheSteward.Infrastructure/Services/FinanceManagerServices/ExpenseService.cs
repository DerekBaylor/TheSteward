using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TheSteward.Core.Dtos.FinanceManagerDtos;
using TheSteward.Core.IRepositories;
using TheSteward.Core.IRepositories.FinanceManagerIRepositories;
using TheSteward.Core.IServices.FinanceManagerIServices;
using TheSteward.Core.Models.FinanceManagerModels;

namespace TheSteward.Infrastructure.Services.FinanceManagerServices;

public class ExpenseService : IExpenseService
{
    private readonly IBaseRepository<Expense> _expenseRepository;
    private readonly IMapper _mapper;

    public ExpenseService(IExpenseRepository expenseRepository, IMapper mapper)
    {
        _expenseRepository = expenseRepository;
        _mapper = mapper;
    }

    public async Task<ExpenseDto> AddAsync(CreateExpenseDto expenseDto)
    {
        if (expenseDto == null)
            throw new ArgumentNullException(nameof(expenseDto));

        var expense = new Expense
        {
            ExpenseId = Guid.NewGuid(),
            ExpenseName = expenseDto.ExpenseName,
            DueDay = expenseDto.DueDay,
            AmountDue = expenseDto.AmountDue,
            DisplayOrder = expenseDto.DisplayOrder,
            BudgetId = expenseDto.BudgetId,
            BudgetCategoryId = expenseDto.BudgetCategoryId,
            CreditId = expenseDto.CreditId,
            InvestmentId = expenseDto.InvestmentId,
        };
        
        await _expenseRepository.AddAsync(expense);
        await _expenseRepository.SaveChangesAsync();
        
        return _mapper.Map<ExpenseDto>(expense);
    }

    public async Task<UpdateExpenseDto> UpdateAsync(UpdateExpenseDto expenseDto)
    {
        if (expenseDto == null)
            throw new ArgumentNullException(nameof(expenseDto));

        var expense = await _expenseRepository.GetByIdAsync(expenseDto.ExpenseId);
        if (expense == null)
            throw new KeyNotFoundException($"Expense with ID {expenseDto.ExpenseId} not found.");
        
        expense.ExpenseName = expenseDto.ExpenseName;
        expense.DueDay = expenseDto.DueDay;
        expense.AmountDue = expenseDto.AmountDue;
        expense.DisplayOrder = expenseDto.DisplayOrder;
        expense.BudgetCategoryId = expenseDto.BudgetCategoryId;
        expense.CreditId = expenseDto.CreditId;
        expense.InvestmentId = expenseDto.InvestmentId;

        await _expenseRepository.UpdateAsync(expense);
        await _expenseRepository.SaveChangesAsync();

        return expenseDto;
    }

    public async Task DeleteAsync(Guid expenseId)
    {
        if (expenseId == Guid.Empty)
            throw new ArgumentException("Expense ID cannot be empty.", nameof(expenseId));

        var expense = await _expenseRepository.GetByIdAsync(expenseId);
        if (expense == null)
            throw new KeyNotFoundException($"Expense with ID {expenseId} not found.");

        await _expenseRepository.DeleteAsync(expense);
        await _expenseRepository.SaveChangesAsync();
    }

    public async Task<ExpenseDto?> GetAsync(Guid expenseId)
    {
        if (expenseId == Guid.Empty)
            throw new ArgumentException("Expense ID cannot be empty.", nameof(expenseId));

        var expense = await _expenseRepository.GetByIdAsync(expenseId);

        return expense == null ? null : _mapper.Map<ExpenseDto>(expense);
    }

    public async Task<ExpenseDto?> GetWithRelatedDataAsync(Guid expenseId)
    {
        if (expenseId == Guid.Empty)
            throw new ArgumentException("Expense ID cannot be empty.", nameof(expenseId));

        var expense = await _expenseRepository.GetAll()
            .Include(e => e.BudgetCategory)
            .Include(e => e.LinkedCredit)
            .Include(e => e.LinkedInvestment)
            .FirstOrDefaultAsync(e => e.ExpenseId == expenseId);

        return expense == null ? null : _mapper.Map<ExpenseDto>(expense);
    }

    public async Task<List<ExpenseDto>> GetAllByBudgetIdAsync(Guid budgetId)
    {
        if (budgetId == Guid.Empty)
            throw new ArgumentException("Budget ID cannot be empty.", nameof(budgetId));

        var expenses = await _expenseRepository.GetAll()
            .Where(e => e.BudgetId == budgetId)
            .OrderBy(e => e.DisplayOrder)
            .ToListAsync();

        return _mapper.Map<List<ExpenseDto>>(expenses);
    }

    public async Task<List<ExpenseDto>> GetAllByCategoryIdAsync(Guid categoryId)
    {
        if (categoryId == Guid.Empty)
            throw new ArgumentException("Category ID cannot be empty.", nameof(categoryId));

        var expenses = await _expenseRepository.GetAll()
            .Where(e => e.BudgetCategoryId == categoryId)
            .OrderBy(e => e.DisplayOrder)
            .ToListAsync();

        return _mapper.Map<List<ExpenseDto>>(expenses);
    }

    public async Task<List<ExpenseDto>> GetAllByBudgetIdWithRelatedDataAsync(Guid budgetId)
    {
        if (budgetId == Guid.Empty)
            throw new ArgumentException("Budget ID cannot be empty.", nameof(budgetId));

        var expenses = await _expenseRepository.GetAll()
            .Where(e => e.BudgetId == budgetId)
            .Include(e => e.BudgetCategory)
            .Include(e => e.LinkedCredit)
            .Include(e => e.LinkedInvestment)
            .OrderBy(e => e.DisplayOrder)
            .ToListAsync();

        return _mapper.Map<List<ExpenseDto>>(expenses);
    }
}