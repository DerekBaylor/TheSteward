using TheSteward.Core.Dtos.FinanceManagerDtos;
using TheSteward.Core.Models.FinanceManagerModels;

namespace TheSteward.Core.IServices.FinanceManagerIServices;

public interface IBudgetService : IBaseService<Budget>
{
    Task<BudgetDto> AddAsync(CreateBudgetDto budget);
    Task<BudgetDto> AddStarterBudget(Guid UserHouseholdId);
    Task DeleteAsync(Guid BudgetId);
    Task<BudgetDto> UpdateAsync(UpdateBudgetDto budget);
    Task<BudgetDto> GetByIdAsync(Guid budgetId);
    Task<List<BudgetDto>> GetBudgetsByUserHouseholdIdAsync(Guid  userHouseholdId);
    Task<BudgetDto> GetByHouseholdAsync(Guid householdId);
    IQueryable<Budget> GetBudgets();
}