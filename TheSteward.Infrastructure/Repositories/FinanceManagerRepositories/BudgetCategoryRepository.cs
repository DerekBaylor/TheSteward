using TheSteward.Core.IRepositories.FinanceManagerIRepositories;
using TheSteward.Core.Models.FinanceManagerModels;
using TheSteward.Infrastructure.Data;

namespace TheSteward.Infrastructure.Repositories.FinanceManagerRepositories;

public class BudgetCategoryRepository : BaseRepository<BudgetCategory>, IBudgetCategoryRepository
{
    public BudgetCategoryRepository(TheStewardContext context) : base(context)
    {
    }
}