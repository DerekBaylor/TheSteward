using Microsoft.EntityFrameworkCore;
using TheSteward.Core.Dtos.TaskManagerDtos;
using TheSteward.Core.IRepositories;
using TheSteward.Core.IServices.TaskManagerIServices;
using TheSteward.Core.MappingExtensions;
using TheSteward.Core.Models.TaskManagerModels;

namespace TheSteward.Infrastructure.Services.TaskManagerServices;

public class RecurrenceRuleService : IRecurrenceRuleService
{
    private readonly IBaseRepository<RecurrenceRule> _recurrenceRuleRepository;

    public RecurrenceRuleService(IBaseRepository<RecurrenceRule> recurrenceRuleRepository)
    {
        _recurrenceRuleRepository = recurrenceRuleRepository;
    }

    public async Task<RecurrenceRuleDto> AddAsync(CreateRecurrenceRuleDto recurrenceRuleDto)
    {
        if (recurrenceRuleDto == null)
            throw new ArgumentNullException(nameof(recurrenceRuleDto));

        var recurrenceRule = recurrenceRuleDto.ToEntity();
        recurrenceRule.RecurrenceRuleId = Guid.NewGuid();
        recurrenceRule.LastGeneratedDateTime = DateTime.UtcNow;
        recurrenceRule.StartDateTime = DateTime.SpecifyKind(recurrenceRuleDto.StartDateTime, DateTimeKind.Utc);
        recurrenceRule.EndDateTime = recurrenceRuleDto.EndDateTime.HasValue
            ? DateTime.SpecifyKind(recurrenceRuleDto.EndDateTime.Value, DateTimeKind.Utc)
            : null;

        await _recurrenceRuleRepository.AddAsync(recurrenceRule);
        await _recurrenceRuleRepository.SaveChangesAsync();

        return recurrenceRule.ToDto();
    }

    public async Task<UpdateRecurrenceRuleDto> UpdateAsync(UpdateRecurrenceRuleDto recurrenceRuleDto)
    {
        if (recurrenceRuleDto == null)
            throw new ArgumentNullException(nameof(recurrenceRuleDto));

        var recurrenceRule = await _recurrenceRuleRepository.GetByIdAsync(recurrenceRuleDto.RecurrenceRuleId);
        if (recurrenceRule == null)
            throw new KeyNotFoundException($"RecurrenceRule with ID {recurrenceRuleDto.RecurrenceRuleId} not found.");

        recurrenceRule.ApplyUpdate(recurrenceRuleDto);
        recurrenceRule.EndDateTime = recurrenceRuleDto.EndDateTime.HasValue
            ? DateTime.SpecifyKind(recurrenceRuleDto.EndDateTime.Value, DateTimeKind.Utc)
            : null;

        await _recurrenceRuleRepository.UpdateAsync(recurrenceRule);
        await _recurrenceRuleRepository.SaveChangesAsync();

        return recurrenceRuleDto;
    }

    public async Task DeleteAsync(Guid recurrenceRuleId)
    {
        if (recurrenceRuleId == Guid.Empty)
            throw new ArgumentException("RecurrenceRule ID cannot be empty.", nameof(recurrenceRuleId));

        var recurrenceRule = await _recurrenceRuleRepository.GetByIdAsync(recurrenceRuleId);
        if (recurrenceRule == null)
            throw new KeyNotFoundException($"RecurrenceRule with ID {recurrenceRuleId} not found.");

        await _recurrenceRuleRepository.DeleteAsync(recurrenceRule);
        await _recurrenceRuleRepository.SaveChangesAsync();
    }

    #region Get Methods

    public async Task<RecurrenceRuleDto?> GetAsync(Guid recurrenceRuleId)
    {
        if (recurrenceRuleId == Guid.Empty)
            throw new ArgumentException("RecurrenceRule ID cannot be empty.", nameof(recurrenceRuleId));

        var recurrenceRule = await _recurrenceRuleRepository.GetByIdAsync(recurrenceRuleId);

        return recurrenceRule?.ToDto();
    }

    public async Task<RecurrenceRuleDto?> GetWithTaskItemsAsync(Guid recurrenceRuleId)
    {
        if (recurrenceRuleId == Guid.Empty)
            throw new ArgumentException("RecurrenceRule ID cannot be empty.", nameof(recurrenceRuleId));

        var recurrenceRule = await _recurrenceRuleRepository.GetAll()
            .Include(r => r.TaskItems)
            .FirstOrDefaultAsync(r => r.RecurrenceRuleId == recurrenceRuleId);

        return recurrenceRule?.ToDto();
    }

    public async Task<List<RecurrenceRuleDto>> GetAllByTaskItemIdAsync(Guid taskItemId)
    {
        if (taskItemId == Guid.Empty)
            throw new ArgumentException("TaskItem ID cannot be empty.", nameof(taskItemId));

        var recurrenceRules = await _recurrenceRuleRepository.GetAll()
            .Where(r => r.TaskItems != null && r.TaskItems.Any(t => t.TaskItemId == taskItemId))
            .ToListAsync();

        return recurrenceRules.ToDtoList();
    }

    #endregion Get Methods
}

