using TheSteward.Core.Dtos.TaskManagerDtos;

namespace TheSteward.Core.IServices.TaskManagerIServices;

public interface ITaskItemCategoryService
{
    /// <summary>
    /// Asynchronously creates a new task item category.
    /// </summary>
    /// <param name="taskItemCategoryDto">The category data for creation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the created category as <see cref="TaskItemCategoryDto"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="taskItemCategoryDto"/> is null.</exception>
    /// <example>
    /// <code>
    /// var createDto = new CreateTaskItemCategoryDto
    /// {
    ///     TaskItemCategoryName = "Kitchen",
    ///     ColorHex = "#FF5733",
    ///     IconName = "kitchen"
    /// };
    /// var category = await taskItemCategoryService.AddAsync(createDto);
    /// </code>
    /// </example>
    Task<TaskItemCategoryDto> AddAsync(CreateTaskItemCategoryDto taskItemCategoryDto);

    /// <summary>
    /// Asynchronously updates an existing task item category.
    /// </summary>
    /// <param name="taskItemCategoryDto">The category data with updated values.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the updated category as <see cref="UpdateTaskItemCategoryDto"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="taskItemCategoryDto"/> is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the category with the specified ID is not found.</exception>
    /// <example>
    /// <code>
    /// var updateDto = new UpdateTaskItemCategoryDto
    /// {
    ///     TaskItemCategoryId = categoryId,
    ///     TaskItemCategoryName = "Kitchen - Updated",
    ///     ColorHex = "#123456",
    ///     IconName = "kitchen_updated"
    /// };
    /// var updated = await taskItemCategoryService.UpdateAsync(updateDto);
    /// </code>
    /// </example>
    Task<UpdateTaskItemCategoryDto> UpdateAsync(UpdateTaskItemCategoryDto taskItemCategoryDto);

    /// <summary>
    /// Asynchronously deletes a task item category by its identifier.
    /// </summary>
    /// <param name="taskItemCategoryId">The unique identifier of the category to delete.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="taskItemCategoryId"/> is empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the category with the specified ID is not found.</exception>
    /// <remarks>
    /// This performs a hard delete. Ensure no TaskItems are still referencing this category before deleting,
    /// or handle the FK constraint accordingly in your database configuration.
    /// </remarks>
    /// <example>
    /// <code>
    /// await taskItemCategoryService.DeleteAsync(categoryId);
    /// </code>
    /// </example>
    Task DeleteAsync(Guid taskItemCategoryId);

    #region Get Methods
    /// <summary>
    /// Asynchronously retrieves a single task item category by its identifier.
    /// </summary>
    /// <param name="taskItemCategoryId">The unique identifier of the category to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the category as <see cref="TaskItemCategoryDto"/>,
    /// or null if not found.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="taskItemCategoryId"/> is empty.</exception>
    /// <remarks>
    /// This method retrieves only the category without its associated TaskItems.
    /// Use <see cref="GetWithTaskItemsAsync"/> to include related task items.
    /// </remarks>
    Task<TaskItemCategoryDto?> GetAsync(Guid taskItemCategoryId);

    /// <summary>
    /// Asynchronously retrieves a task item category with all associated task items.
    /// </summary>
    /// <param name="taskItemCategoryId">The unique identifier of the category to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the category
    /// with its TaskItems as <see cref="TaskItemCategoryDto"/>, or null if not found.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="taskItemCategoryId"/> is empty.</exception>
    Task<TaskItemCategoryDto?> GetWithTaskItemsAsync(Guid taskItemCategoryId);

    /// <summary>
    /// Asynchronously retrieves all task item categories.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of all
    /// categories ordered by name.
    /// </returns>
    Task<List<TaskItemCategoryDto>> GetAllAsync();

    /// <summary>
    /// Asynchronously retrieves a task item category by name.
    /// </summary>
    /// <param name="taskItemCategoryName">The name of the category to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the matching
    /// category as <see cref="TaskItemCategoryDto"/>.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="taskItemCategoryName"/> is null or whitespace.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when no category with the given name is found.</exception>
    Task<TaskItemCategoryDto> GetByNameAsync(string taskItemCategoryName);
    #endregion Get Methods
}