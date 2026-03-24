using TheSteward.Core.Dtos.TaskManagerDtos;
using static TheSteward.Core.Utils.TaskManagerUtils.TaskManagerConstants;

namespace TheSteward.Core.IServices.TaskManagerIServices;

public interface ITaskItemOccurrenceService
{
    /// <summary>
    /// Asynchronously creates a new task item occurrence.
    /// </summary>
    /// <param name="taskItemOccurrenceDto">The occurrence data for creation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the created occurrence as <see cref="TaskItemOccurrenceDto"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="taskItemOccurrenceDto"/> is null.</exception>
    /// <example>
    /// <code>
    /// var createDto = new CreateTaskItemOccurrenceDto
    /// {
    ///     TaskItemId = taskItemId,
    ///     ScheduledDateTime = DateTime.UtcNow.AddDays(7)
    /// };
    /// var occurrence = await taskItemOccurrenceService.AddAsync(createDto);
    /// </code>
    /// </example>
    Task<TaskItemOccurrenceDto> AddAsync(CreateTaskItemOccurrenceDto taskItemOccurrenceDto);

    /// <summary>
    /// Asynchronously updates an existing task item occurrence.
    /// </summary>
    /// <param name="taskItemOccurrenceDto">The occurrence data with updated values.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the updated occurrence as <see cref="UpdateTaskItemOccurrenceDto"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="taskItemOccurrenceDto"/> is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the occurrence with the specified ID is not found.</exception>
    Task<UpdateTaskItemOccurrenceDto> UpdateAsync(UpdateTaskItemOccurrenceDto taskItemOccurrenceDto);

    /// <summary>
    /// Asynchronously marks a task item occurrence as completed.
    /// </summary>
    /// <param name="taskItemOccurrenceId">The unique identifier of the occurrence to complete.</param>
    /// <param name="completedByUserHouseholdId">The unique identifier of the user household completing the occurrence.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the updated occurrence as <see cref="TaskItemOccurrenceDto"/>.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="taskItemOccurrenceId"/> or <paramref name="completedByUserHouseholdId"/> is empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the occurrence with the specified ID is not found.</exception>
    /// <example>
    /// <code>
    /// var completed = await taskItemOccurrenceService.CompleteAsync(occurrenceId, userHouseholdId);
    /// </code>
    /// </example>
    Task<TaskItemOccurrenceDto> CompleteAsync(Guid taskItemOccurrenceId, Guid completedByUserHouseholdId);

    /// <summary>
    /// Asynchronously marks a task item occurrence as skipped.
    /// </summary>
    /// <param name="taskItemOccurrenceId">The unique identifier of the occurrence to skip.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the updated occurrence as <see cref="TaskItemOccurrenceDto"/>.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="taskItemOccurrenceId"/> is empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the occurrence with the specified ID is not found.</exception>
    Task<TaskItemOccurrenceDto> SkipAsync(Guid taskItemOccurrenceId);

    /// <summary>
    /// Asynchronously deletes a task item occurrence by its identifier.
    /// </summary>
    /// <param name="taskItemOccurrenceId">The unique identifier of the occurrence to delete.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="taskItemOccurrenceId"/> is empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the occurrence with the specified ID is not found.</exception>
    Task DeleteAsync(Guid taskItemOccurrenceId);

    #region Get Methods
    /// <summary>
    /// Asynchronously retrieves a single task item occurrence by its identifier.
    /// </summary>
    /// <param name="taskItemOccurrenceId">The unique identifier of the occurrence to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the occurrence as <see cref="TaskItemOccurrenceDto"/>,
    /// or null if not found.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="taskItemOccurrenceId"/> is empty.</exception>
    /// <remarks>
    /// This method retrieves only the occurrence without its associated TaskItem.
    /// Use <see cref="GetWithTaskItemAsync"/> to include the related task item.
    /// </remarks>
    Task<TaskItemOccurrenceDto?> GetAsync(Guid taskItemOccurrenceId);

    /// <summary>
    /// Asynchronously retrieves a task item occurrence with its associated task item.
    /// </summary>
    /// <param name="taskItemOccurrenceId">The unique identifier of the occurrence to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the occurrence with
    /// its TaskItem as <see cref="TaskItemOccurrenceDto"/>, or null if not found.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="taskItemOccurrenceId"/> is empty.</exception>
    Task<TaskItemOccurrenceDto?> GetWithTaskItemAsync(Guid taskItemOccurrenceId);

    /// <summary>
    /// Asynchronously retrieves all occurrences for a specific task item.
    /// </summary>
    /// <param name="taskItemId">The unique identifier of the task item.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of all occurrences
    /// for the specified task item, ordered by scheduled date ascending.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="taskItemId"/> is empty.</exception>
    Task<List<TaskItemOccurrenceDto>> GetAllByTaskItemIdAsync(Guid taskItemId);

    /// <summary>
    /// Asynchronously retrieves all occurrences for a specific task item filtered by status.
    /// </summary>
    /// <param name="taskItemId">The unique identifier of the task item.</param>
    /// <param name="status">The status to filter by.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of occurrences
    /// matching the specified status, ordered by scheduled date ascending.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="taskItemId"/> is empty.</exception>
    Task<List<TaskItemOccurrenceDto>> GetAllByTaskItemIdAndStatusAsync(Guid taskItemId, TaskItemStatus status);

    /// <summary>
    /// Asynchronously retrieves all occurrences assigned to a specific user household within a date range.
    /// </summary>
    /// <param name="userHouseholdId">The unique identifier of the user household.</param>
    /// <param name="startDate">The start of the date range.</param>
    /// <param name="endDate">The end of the date range.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of occurrences
    /// within the specified date range, ordered by scheduled date ascending.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="userHouseholdId"/> is empty or <paramref name="startDate"/> is after <paramref name="endDate"/>.</exception>
    Task<List<TaskItemOccurrenceDto>> GetAllByUserHouseholdIdAndDateRangeAsync(Guid userHouseholdId, DateTime startDate, DateTime endDate);
    #endregion Get Methods
}