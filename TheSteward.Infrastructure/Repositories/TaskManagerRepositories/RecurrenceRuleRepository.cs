using TheSteward.Core.IRepositories.ITaskManagerRepositories;
using TheSteward.Core.Models.TaskManagerModels;
using TheSteward.Infrastructure.Data;

namespace TheSteward.Infrastructure.Repositories.TaskManagerRepositories;

public class RecurrenceRuleRepository : BaseRepository<RecurrenceRule>, IRecurrenceRuleRepository
{
    public RecurrenceRuleRepository(TheStewardContext context) : base(context)
    {
    }
}
