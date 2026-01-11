using TheSteward.Core.IRepositories.FinanceManagerIRepositories;
using TheSteward.Core.Models.FinanceManagerModels;
using TheSteward.Infrastructure.Data;

namespace TheSteward.Infrastructure.Repositories.FinanceManagerRepositories;

public class InvestmentRepository : BaseRepository<Investment>, IInvestmentRepository
{
    InvestmentRepository(TheStewardContext context) : base(context)
    {
    }
}