using TheSteward.Core.IRepositories.HouseholdIRepositories;
using TheSteward.Core.Models.HouseholdModels;
using TheSteward.Infrastructure.Data;

namespace TheSteward.Infrastructure.Repositories.HouseholdRepositories;

public class HouseholdRepository : BaseRepository<Household>, IHouseholdRepository
{
    public HouseholdRepository(TheStewardContext context) : base(context)
    {
    }
}
