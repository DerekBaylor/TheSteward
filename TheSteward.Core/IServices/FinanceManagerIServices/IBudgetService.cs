namespace TheSteward.Core.IServices.FinanceManagerIServices;

public interface IBudgetService
{
    Task AddAsync();
    Task DeleteAsync();
    Task UpdateAsync();
    Task GetByIdAsync();
    Task GetByUserHouseholdIdAsync();
    Task GetByHouseholdAsync();
}