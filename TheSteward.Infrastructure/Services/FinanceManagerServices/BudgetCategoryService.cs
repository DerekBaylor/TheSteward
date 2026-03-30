using Microsoft.EntityFrameworkCore;
using TheSteward.Core.Dtos.FinanceManagerDtos;
using TheSteward.Core.IRepositories.FinanceManagerIRepositories;
using TheSteward.Core.IServices.FinanceManagerIServices;
using TheSteward.Core.MappingExtensions;
using TheSteward.Core.Models.FinanceManagerModels;

namespace TheSteward.Infrastructure.Services.FinanceManagerServices;

public class BudgetCategoryService : IBudgetCategoryService
{
    private readonly IBudgetCategoryRepository _budgetCategoryRepository;

    public BudgetCategoryService(IBudgetCategoryRepository budgetCategoryRepository)
    {
        _budgetCategoryRepository = budgetCategoryRepository;
    }

    public async Task<BudgetCategoryDto> AddAsync(CreateBudgetCategoryDto categoryDto)
    {
        if (categoryDto == null)
            throw new ArgumentNullException(nameof(categoryDto));

        var categoryId = Guid.NewGuid();
        var category = categoryDto.ToEntity(categoryId);

        await _budgetCategoryRepository.AddAsync(category);
        await _budgetCategoryRepository.SaveChangesAsync();

        return category.ToDto();
    }

    public async Task<BudgetCategoryDto> UpdateAsync(UpdateBudgetCategoryDto categoryDto)
    {
        if (categoryDto == null)
            throw new ArgumentNullException(nameof(categoryDto));

        var category = await _budgetCategoryRepository.GetByIdAsync(categoryDto.BudgetCategoryId);
        if (category == null)
            throw new KeyNotFoundException($"Budget category with ID {categoryDto.BudgetCategoryId} not found.");

        category.ApplyUpdate(categoryDto);

        await _budgetCategoryRepository.UpdateAsync(category);
        await _budgetCategoryRepository.SaveChangesAsync();

        return category.ToDto();
    }

    public async Task DeleteAsync(Guid categoryId)
    {
        if (categoryId == Guid.Empty)
            throw new ArgumentException("Budget category ID cannot be empty.", nameof(categoryId));

        var category = await _budgetCategoryRepository.GetByIdAsync(categoryId);
        if (category == null)
            throw new KeyNotFoundException($"Budget category with ID {categoryId} not found.");

        await _budgetCategoryRepository.DeleteAsync(category);
        await _budgetCategoryRepository.SaveChangesAsync();
    }

    #region Get Methods

    public async Task<BudgetCategoryDto?> GetAsync(Guid categoryId)
    {
        if (categoryId == Guid.Empty)
            throw new ArgumentException("Budget category ID cannot be empty.", nameof(categoryId));

        var category = await _budgetCategoryRepository.GetAll()
            .Include(c => c.BudgetSubCategories)
            .FirstOrDefaultAsync(c => c.BudgetCategoryId == categoryId);

        return category?.ToDto();
    }

    public async Task<List<BudgetCategoryDto>> GetAllByBudgetIdAsync(Guid budgetId)
    {
        if (budgetId == Guid.Empty)
            throw new ArgumentException("Budget ID cannot be empty.", nameof(budgetId));

        var categories = await _budgetCategoryRepository.GetAll()
            .Where(c => c.BudgetId == budgetId)
            .Include(c => c.BudgetSubCategories)
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();

        return categories.ToDtoList();
    }

    public async Task<BudgetCategoryDto> GetByIdOrCreateAsync(Guid budgetId, string categoryName)
    {
        if (budgetId == Guid.Empty)
            throw new ArgumentException("Budget ID cannot be empty.", nameof(budgetId));

        if (string.IsNullOrWhiteSpace(categoryName))
            throw new ArgumentException("Category name cannot be null or whitespace.", nameof(categoryName));

        var existing = await _budgetCategoryRepository.GetAll()
            .FirstOrDefaultAsync(c => c.BudgetId == budgetId
                                   && c.BudgetCategoryName == categoryName);

        if (existing != null)
            return existing.ToDto();

        var displayOrder = await _budgetCategoryRepository.GetAll()
            .CountAsync(c => c.BudgetId == budgetId);

        var newCategory = new BudgetCategory
        {
            BudgetCategoryId = Guid.NewGuid(),
            BudgetCategoryName = categoryName,
            BudgetId = budgetId,
            DisplayOrder = displayOrder
        };

        await _budgetCategoryRepository.AddAsync(newCategory);
        await _budgetCategoryRepository.SaveChangesAsync();

        return newCategory.ToDto();
    }

    #endregion Get Methods
}


