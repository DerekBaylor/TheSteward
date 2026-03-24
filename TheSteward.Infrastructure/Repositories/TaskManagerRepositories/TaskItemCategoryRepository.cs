using TheSteward.Core.Models.TaskManagerModels;
using TheSteward.Core.IRepositories.ITaskManagerRepositories;
using TheSteward.Infrastructure.Data;

namespace TheSteward.Infrastructure.Repositories.TaskManagerRepositories;

public class TaskItemCategoryRepository : BaseRepository<TaskItemCategory>, ITaskItemCategoryRepository
{
    public TaskItemCategoryRepository(TheStewardContext context) : base(context)
    {
    }
}
