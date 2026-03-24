using TheSteward.Core.IRepositories.ITaskManagerRepositories;
using TheSteward.Core.Models.TaskManagerModels;
using TheSteward.Infrastructure.Data;

namespace TheSteward.Infrastructure.Repositories.TaskManagerRepositories;

public class TaskItemOccurrenceRepository : BaseRepository<TaskItemOccurrence>, ITaskItemOccurenceRepository
{
    public TaskItemOccurrenceRepository(TheStewardContext context) : base(context)
    {
    }
}
