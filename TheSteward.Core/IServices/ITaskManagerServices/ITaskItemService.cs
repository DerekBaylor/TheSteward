using TheSteward.Core.Dtos.TaskManagerDtos;
using TheSteward.Core.Utils.TaskManagerUtils;
using static TheSteward.Core.Utils.TaskManagerUtils.TaskManagerConstants;

namespace TheSteward.Core.IServices.TaskManagerIServices;

public interface ITaskItemService
{
    #region Create Methods
    /// <summary>
    /// Asynchronously creates a new task item.
    /// </summary>
    /// <param name="taskItemDto">The task item data for creation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the created task item as <see cref="TaskItemDto"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="taskItemDto"/> is null.</exception>
    /// <example>
    /// <code>
    /// var createDto = new CreateTaskItemDto
    /// {
    ///     TaskItemName = "Clean Kitchen",
    ///     Priority = TaskItemPriority.Medium,
    ///     DueDate = DateTime.UtcNow.AddDays(1),
    ///     TaskItemCategoryId = categoryId,
    ///     CreatedByUserHouseholdId = userHouseholdId
    /// };
    /// var taskItem = await taskItemService.AddAsync(createDto);
    /// </code>
    /// </example>
    Task<TaskItemDto> AddAsync(CreateTaskItemDto taskItemDto);

    Task AddStandardTasksAsync(Guid userHouseholdId, IEnumerable<StandardTaskDefinition> selectedTasks, IEnumerable<TaskItemDto> existingTasks);
    #endregion Create

    /// <summary>
    /// Asynchronously updates an existing task item.
    /// </summary>
    /// <param name="taskItemDto">The task item data with updated values.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the updated task item as <see cref="UpdateTaskItemDto"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="taskItemDto"/> is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the task item with the specified ID is not found.</exception>
    /// <example>
    /// <code>
    /// var updateDto = new UpdateTaskItemDto
    /// {
    ///     TaskItemId = taskItemId,
    ///     TaskItemName = "Clean Kitchen - Updated",
    ///     Status = TaskItemStatus.InProgress,
    ///     Priority = TaskItemPriority.High
    /// };
    /// var updated = await taskItemService.UpdateAsync(updateDto);
    /// </code>
    /// </example>
    Task<UpdateTaskItemDto> UpdateAsync(UpdateTaskItemDto taskItemDto);

    /// <summary>
    /// Asynchronously marks a task item as completed.
    /// </summary>
    /// <param name="taskItemId">The unique identifier of the task item to complete.</param>
    /// <param name="completedByUserHouseholdId">The unique identifier of the user household completing the task.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the updated task as <see cref="TaskItemDto"/>.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="taskItemId"/> or <paramref name="completedByUserHouseholdId"/> is empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the task item with the specified ID is not found.</exception>
    Task<TaskItemDto> CompleteAsync(Guid taskItemId, Guid completedByUserHouseholdId);

    /// <summary>
    /// Asynchronously deletes a task item by its identifier.
    /// </summary>
    /// <param name="taskItemId">The unique identifier of the task item to delete.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="taskItemId"/> is empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the task item with the specified ID is not found.</exception>
    /// <remarks>
    /// This performs a hard delete. Consider using <see cref="ArchiveAsync"/> instead to preserve task history.
    /// </remarks>
    Task DeleteAsync(Guid taskItemId);

    /// <summary>
    /// Asynchronously archives a task item by its identifier, preserving it for historical reference.
    /// </summary>
    /// <param name="taskItemId">The unique identifier of the task item to archive.</param>
    /// <returns>A task that represents the asynchronous archive operation.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="taskItemId"/> is empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the task item with the specified ID is not found.</exception>
    Task ArchiveAsync(Guid taskItemId);

    #region Get Methods
    /// <summary>
    /// Asynchronously retrieves a single task item by its identifier.
    /// </summary>
    /// <param name="taskItemId">The unique identifier of the task item to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the task item as <see cref="TaskItemDto"/>,
    /// or null if not found.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="taskItemId"/> is empty.</exception>
    /// <remarks>
    /// This method retrieves only the task item without related data.
    /// Use <see cref="GetWithRelatedDataAsync"/> to include related entities.
    /// </remarks>
    Task<TaskItemDto?> GetAsync(Guid taskItemId);

    /// <summary>
    /// Asynchronously retrieves a task item with all related data.
    /// </summary>
    /// <param name="taskItemId">The unique identifier of the task item to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the task item with
    /// its Category, RecurrenceRule, and RelatedExpense as <see cref="TaskItemDto"/>, or null if not found.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="taskItemId"/> is empty.</exception>
    Task<TaskItemDto?> GetWithRelatedDataAsync(Guid taskItemId);

    /// <summary>
    /// Asynchronously retrieves all task items assigned to a specific user household.
    /// </summary>
    /// <param name="userHouseholdId">The unique identifier of the user household.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of task items
    /// assigned to the specified user household, ordered by due date.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="userHouseholdId"/> is empty.</exception>
    Task<List<TaskItemDto>> GetAllByAssignedUserHouseholdIdAsync(Guid userHouseholdId);

    /// <summary>
    /// Asynchronously retrieves all task items for a specific category.
    /// </summary>
    /// <param name="taskItemCategoryId">The unique identifier of the category.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of task items
    /// in the specified category, ordered by due date.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="taskItemCategoryId"/> is empty.</exception>
    Task<List<TaskItemDto>> GetAllByCategoryIdAsync(Guid taskItemCategoryId);

    /// <summary>
    /// Asynchronously retrieves all task items with a specific status for a given user household.
    /// </summary>
    /// <param name="userHouseholdId">The unique identifier of the user household.</param>
    /// <param name="status">The status to filter by.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of task items
    /// matching the specified status, ordered by due date.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="userHouseholdId"/> is empty.</exception>
    Task<List<TaskItemDto>> GetAllByStatusAsync(Guid userHouseholdId, TaskItemStatus status);

    /// <summary>
    /// Asynchronously retrieves all active (non-archived) task items created by a specific user household.
    /// </summary>
    /// <param name="createdByUserHouseholdId">The unique identifier of the creating user household.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of active task items
    /// ordered by due date.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="createdByUserHouseholdId"/> is empty.</exception>
    Task<List<TaskItemDto>> GetAllByCreatedByUserHouseholdIdAsync(Guid createdByUserHouseholdId);

    /// <summary>
    /// Retrieves all tasks for a household that are visible to the requesting UserHousehold.
    /// Public tasks are visible to all members. Private tasks are only visible to the
    /// UserHousehold that created them or the UserHousehold they are assigned to.
    /// </summary>
    /// <param name="householdId">The household to retrieve tasks for.</param>
    /// <param name="requestingUserHouseholdId">The UserHousehold making the request.</param>
    /// <returns>A list of visible, non-archived <see cref="TaskItemDto"/> records.</returns>
    Task<List<TaskItemDto>> GetAllByHouseholdIdAsync(Guid householdId, Guid requestingUserHouseholdId);
    #endregion Get Methods
}