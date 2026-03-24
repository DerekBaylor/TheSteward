using TheSteward.Core.Dtos.TaskManagerDtos;

namespace TheSteward.Core.IServices.TaskManagerIServices;

public interface IRecurrenceRuleService
{
    /// <summary>
    /// Asynchronously creates a new recurrence rule.
    /// </summary>
    /// <param name="recurrenceRuleDto">The recurrence rule data for creation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the created recurrence rule as <see cref="RecurrenceRuleDto"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="recurrenceRuleDto"/> is null.</exception>
    /// <example>
    /// <code>
    /// var createDto = new CreateRecurrenceRuleDto
    /// {
    ///     RecurrenceFrequency = RecurrenceFrequency.Weekly,
    ///     RecurrenceDays = DaysOfWeek.Monday | DaysOfWeek.Thursday,
    ///     StartDateTime = DateTime.UtcNow
    /// };
    /// var rule = await recurrenceRuleService.AddAsync(createDto);
    /// </code>
    /// </example>
    Task<RecurrenceRuleDto> AddAsync(CreateRecurrenceRuleDto recurrenceRuleDto);

    /// <summary>
    /// Asynchronously updates an existing recurrence rule.
    /// </summary>
    /// <param name="recurrenceRuleDto">The recurrence rule data with updated values.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the updated recurrence rule as <see cref="UpdateRecurrenceRuleDto"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="recurrenceRuleDto"/> is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the recurrence rule with the specified ID is not found.</exception>
    /// <example>
    /// <code>
    /// var updateDto = new UpdateRecurrenceRuleDto
    /// {
    ///     RecurrenceRuleId = ruleId,
    ///     RecurrenceFrequency = RecurrenceFrequency.BiWeekly,
    ///     EndDateTime = DateTime.UtcNow.AddMonths(6)
    /// };
    /// var updated = await recurrenceRuleService.UpdateAsync(updateDto);
    /// </code>
    /// </example>
    Task<UpdateRecurrenceRuleDto> UpdateAsync(UpdateRecurrenceRuleDto recurrenceRuleDto);

    /// <summary>
    /// Asynchronously deletes a recurrence rule by its identifier.
    /// </summary>
    /// <param name="recurrenceRuleId">The unique identifier of the recurrence rule to delete.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="recurrenceRuleId"/> is empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the recurrence rule with the specified ID is not found.</exception>
    /// <remarks>
    /// This performs a hard delete. Note that any TaskItems linked to this rule will have their
    /// RecurrenceId set to null if you have the FK configured as optional.
    /// </remarks>
    /// <example>
    /// <code>
    /// await recurrenceRuleService.DeleteAsync(recurrenceRuleId);
    /// </code>
    /// </example>
    Task DeleteAsync(Guid recurrenceRuleId);

    #region Get Methods
    /// <summary>
    /// Asynchronously retrieves a single recurrence rule by its identifier.
    /// </summary>
    /// <param name="recurrenceRuleId">The unique identifier of the recurrence rule to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the recurrence rule as <see cref="RecurrenceRuleDto"/>,
    /// or null if not found.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="recurrenceRuleId"/> is empty.</exception>
    /// <remarks>
    /// This method retrieves only the recurrence rule without its associated TaskItems.
    /// Use <see cref="GetWithTaskItemsAsync"/> to include related task items.
    /// </remarks>
    Task<RecurrenceRuleDto?> GetAsync(Guid recurrenceRuleId);

    /// <summary>
    /// Asynchronously retrieves a recurrence rule with all associated task items.
    /// </summary>
    /// <param name="recurrenceRuleId">The unique identifier of the recurrence rule to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the recurrence rule
    /// with its TaskItems as <see cref="RecurrenceRuleDto"/>, or null if not found.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="recurrenceRuleId"/> is empty.</exception>
    Task<RecurrenceRuleDto?> GetWithTaskItemsAsync(Guid recurrenceRuleId);

    /// <summary>
    /// Asynchronously retrieves all recurrence rules associated with a specific task item.
    /// </summary>
    /// <param name="taskItemId">The unique identifier of the task item.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of
    /// recurrence rules associated with the specified task item.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="taskItemId"/> is empty.</exception>
    Task<List<RecurrenceRuleDto>> GetAllByTaskItemIdAsync(Guid taskItemId);
    #endregion Get Methods
}