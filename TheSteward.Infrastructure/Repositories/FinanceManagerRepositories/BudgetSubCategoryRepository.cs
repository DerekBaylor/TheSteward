using TheSteward.Core.IRepositories.FinanceManagerIRepositories;
using TheSteward.Core.Models.FinanceManagerModels;
using TheSteward.Infrastructure.Data;

namespace TheSteward.Infrastructure.Repositories.FinanceManagerRepositories;

public class BudgetSubCategoryRepository : BaseRepository<BudgetSubCategory>, IBudgetSubCategoryRepository
{
    public BudgetSubCategoryRepository(TheStewardContext context) : base(context)
    {
    }
}