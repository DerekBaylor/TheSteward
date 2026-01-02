using TheSteward.Core.IRepositories.HouseholdIRepositories;
using TheSteward.Core.Models.HouseholdModels;
using TheSteward.Infrastructure.Data;

namespace TheSteward.Infrastructure.Repositories.HouseholdRepositories;

public class UserHouseholdRepository : BaseRepository<UserHousehold>, IUserHouseholdRepository
{
    public UserHouseholdRepository(TheStewardContext context) : base(context)
    {
    }
}
