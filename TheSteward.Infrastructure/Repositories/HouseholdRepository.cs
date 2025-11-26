using TheSteward.Core.IRepositories;
using TheSteward.Core.Models.HouseholdModels;
using TheSteward.Infrastructure.Data;

namespace TheSteward.Infrastructure.Repositories;

public class HouseholdRepository : BaseRepository<Household>, IHouseholdRepository
{
    public HouseholdRepository(TheStewardContext context) : base(context)
    {
    }
}
