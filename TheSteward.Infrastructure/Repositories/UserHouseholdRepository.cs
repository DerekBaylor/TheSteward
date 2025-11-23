using TheSteward.Core.IRepositories;
using TheSteward.Core.Models.HouseholdModels;
using TheSteward.Infrastructure.Data;

namespace TheSteward.Infrastructure.Repositories;

public class UserHouseholdRepository : BaseRepository<UserHousehold>, IUserHouseholdRepository
{
    public UserHouseholdRepository(TheStewardContext context) : base(context)
    {
    }
}
