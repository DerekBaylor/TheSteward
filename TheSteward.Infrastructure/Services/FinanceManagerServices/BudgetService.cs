using TheSteward.Core.Dtos.FinanceManagerDtos;
using TheSteward.Core.IRepositories;
using TheSteward.Core.IRepositories.FinanceManagerIRepositories;
using TheSteward.Core.IServices.FinanceManagerIServices;
using TheSteward.Core.Models.FinanceManagerModels;

namespace TheSteward.Infrastructure.Services.FinanceManagerServices;

public class BudgetService : BaseService<Budget>, IBudgetService
{
    private readonly IBudgetRepository _budgetRepository;

    public BudgetService(IBaseRepository<Budget> baseRepository, IBudgetRepository budgetRepository) : base(baseRepository)
    {
        _budgetRepository = budgetRepository;
    }

    public Task<BudgetDto> AddAsync(CreateBudgetDto budget)
    {
        throw new NotImplementedException();
    }

    public Task<BudgetDto> AddStarterBudget(Guid UserHouseholdId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid BudgetId)
    {
        throw new NotImplementedException();
    }

    public Task<BudgetDto> UpdateAsync(UpdateBudgetDto budget)
    {
        throw new NotImplementedException();
    }

    public Task<BudgetDto> GetByIdAsync(Guid budgetId)
    {
        throw new NotImplementedException();
    }

    public Task<List<BudgetDto>> GetBudgetsByUserHouseholdIdAsync(Guid userHouseholdId)
    {
        throw new NotImplementedException();
    }

    public Task<BudgetDto> GetByHouseholdAsync(Guid householdId)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Budget> GetBudgets()
    {
        throw new NotImplementedException();
    }
}