using TheSteward.Core.IRepositories.FinanceManagerIRepositories;
using TheSteward.Core.Models.FinanceManagerModels;
using TheSteward.Infrastructure.Data;

namespace TheSteward.Infrastructure.Repositories.FinanceManagerRepositories;

public class CreditRepository : BaseRepository<Credit>, ICreditRepository
{
    public CreditRepository(TheStewardContext context) : base(context)
    {
    }
}