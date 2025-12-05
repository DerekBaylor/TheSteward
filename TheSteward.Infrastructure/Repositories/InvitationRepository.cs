using TheSteward.Core.IRepositories;
using TheSteward.Core.Models.HouseholdModels;
using TheSteward.Infrastructure.Data;

namespace TheSteward.Infrastructure.Repositories;

public class InvitationRepository : BaseRepository<HouseholdInvitation>, IInvitationRepository
{
    public InvitationRepository(TheStewardContext context) : base(context)
    {
    }
}
