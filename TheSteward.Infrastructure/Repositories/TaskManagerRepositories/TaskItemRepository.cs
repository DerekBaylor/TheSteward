using TheSteward.Core.IRepositories.ITaskManagerRepositories;
using TheSteward.Core.Models.TaskManagerModels;
using TheSteward.Infrastructure.Data;

namespace TheSteward.Infrastructure.Repositories.TaskManagerRepositories;

public class TaskItemRepository : BaseRepository<TaskItem>, ITaskItemRepository
{
    public TaskItemRepository(TheStewardContext context) : base(context)
    {
    }
}
