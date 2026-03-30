using Microsoft.EntityFrameworkCore;
using TheSteward.Core.Dtos.FinanceManagerDtos;
using TheSteward.Core.IRepositories.FinanceManagerIRepositories;
using TheSteward.Core.IServices.FinanceManagerIServices;
using TheSteward.Core.MappingExtensions;
using TheSteward.Core.Models.FinanceManagerModels;

namespace TheSteward.Infrastructure.Services.FinanceManagerServices;

public class BudgetSubCategoryService : IBudgetSubCategoryService
{
    private readonly IBudgetSubCategoryRepository _budgetSubCategoryRepository;

    public BudgetSubCategoryService(IBudgetSubCategoryRepository budgetSubCategoryRepository)
    {
        _budgetSubCategoryRepository = budgetSubCategoryRepository;
    }

    public async Task<BudgetSubCategoryDto> AddAsync(CreateBudgetSubCategoryDto subCategoryDto)
    {
        if (subCategoryDto == null)
            throw new ArgumentNullException(nameof(subCategoryDto));

        var subCategoryId = Guid.NewGuid();
        var subCategory = subCategoryDto.ToEntity(subCategoryId);

        await _budgetSubCategoryRepository.AddAsync(subCategory);
        await _budgetSubCategoryRepository.SaveChangesAsync();

        return subCategory.ToDto();
    }

    public async Task<BudgetSubCategoryDto> UpdateAsync(UpdateBudgetSubCategoryDto subCategoryDto)
    {
        if (subCategoryDto == null)
            throw new ArgumentNullException(nameof(subCategoryDto));

        var subCategory = await _budgetSubCategoryRepository.GetByIdAsync(subCategoryDto.BudgetSubCategoryId);
        if (subCategory == null)
            throw new KeyNotFoundException($"Budget subcategory with ID {subCategoryDto.BudgetSubCategoryId} not found.");

        subCategory.ApplyUpdate(subCategoryDto);

        await _budgetSubCategoryRepository.UpdateAsync(subCategory);
        await _budgetSubCategoryRepository.SaveChangesAsync();

        return subCategory.ToDto();
    }

    public async Task DeleteAsync(Guid subCategoryId)
    {
        if (subCategoryId == Guid.Empty)
            throw new ArgumentException("Budget subcategory ID cannot be empty.", nameof(subCategoryId));

        var subCategory = await _budgetSubCategoryRepository.GetByIdAsync(subCategoryId);
        if (subCategory == null)
            throw new KeyNotFoundException($"Budget subcategory with ID {subCategoryId} not found.");

        await _budgetSubCategoryRepository.DeleteAsync(subCategory);
        await _budgetSubCategoryRepository.SaveChangesAsync();
    }

    #region Get Methods

    public async Task<BudgetSubCategoryDto?> GetAsync(Guid subCategoryId)
    {
        if (subCategoryId == Guid.Empty)
            throw new ArgumentException("Budget subcategory ID cannot be empty.", nameof(subCategoryId));

        var subCategory = await _budgetSubCategoryRepository.GetByIdAsync(subCategoryId);

        return subCategory?.ToDto();
    }

    public async Task<List<BudgetSubCategoryDto>> GetAllByCategoryIdAsync(Guid categoryId)
    {
        if (categoryId == Guid.Empty)
            throw new ArgumentException("Category ID cannot be empty.", nameof(categoryId));

        var subCategories = await _budgetSubCategoryRepository.GetAll()
            .Where(sc => sc.BudgetCategoryId == categoryId)
            .OrderBy(sc => sc.DisplayOrder)
            .ToListAsync();

        return subCategories.ToDtoList();
    }

    public async Task<List<BudgetSubCategoryDto>> GetAllByBudgetIdAsync(Guid budgetId)
    {
        if (budgetId == Guid.Empty)
            throw new ArgumentException("Budget ID cannot be empty.", nameof(budgetId));

        var subCategories = await _budgetSubCategoryRepository.GetAll()
            .Where(sc => sc.BudgetId == budgetId)
            .OrderBy(sc => sc.DisplayOrder)
            .ToListAsync();

        return subCategories.ToDtoList();
    }

    public async Task<BudgetSubCategoryDto> GetByIdOrCreateSubCategoryAsync(
        Guid budgetId, Guid budgetCategoryId, string subCategoryName)
    {
        if (budgetId == Guid.Empty)
            throw new ArgumentException("Budget ID cannot be empty.", nameof(budgetId));

        if (budgetCategoryId == Guid.Empty)
            throw new ArgumentException("Budget Category ID cannot be empty.", nameof(budgetCategoryId));

        if (string.IsNullOrWhiteSpace(subCategoryName))
            throw new ArgumentException("SubCategory name cannot be null or whitespace.", nameof(subCategoryName));

        var existing = await _budgetSubCategoryRepository.GetAll()
            .FirstOrDefaultAsync(sc => sc.BudgetId == budgetId
                                    && sc.BudgetCategoryId == budgetCategoryId
                                    && sc.BudgetSubCategoryName == subCategoryName);

        if (existing != null)
            return existing.ToDto();

        var displayOrder = await _budgetSubCategoryRepository.GetAll()
            .CountAsync(sc => sc.BudgetCategoryId == budgetCategoryId);

        var newSubCategory = new BudgetSubCategory
        {
            BudgetSubCategoryId = Guid.NewGuid(),
            BudgetSubCategoryName = subCategoryName,
            BudgetId = budgetId,
            BudgetCategoryId = budgetCategoryId,
            DisplayOrder = displayOrder
        };

        await _budgetSubCategoryRepository.AddAsync(newSubCategory);
        await _budgetSubCategoryRepository.SaveChangesAsync();

        return newSubCategory.ToDto();
    }

    #endregion Get Methods
}


