using TheSteward.Core.IRepositories.HouseholdIRepositories;
using TheSteward.Core.Models.HouseholdModels;
using TheSteward.Infrastructure.Data;

namespace TheSteward.Infrastructure.Repositories.HouseholdRepositories;

public class InvitationRepository : BaseRepository<HouseholdInvitation>, IInvitationRepository
{
    public InvitationRepository(TheStewardContext context) : base(context)
    {
    }
}
