using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TheSteward.Core.Dtos.FinanceManagerDtos;
using TheSteward.Core.IRepositories;
using TheSteward.Core.IRepositories.FinanceManagerIRepositories;
using TheSteward.Core.IServices.FinanceManagerIServices;
using TheSteward.Core.Models.FinanceManagerModels;

namespace TheSteward.Infrastructure.Services.FinanceManagerServices;

public class BudgetCategoryService : BaseService<BudgetCategory>, IBudgetCategoryService
{
    private readonly IBudgetCategoryRepository _budgetCategoryRepository;
    private readonly IMapper _mapper;
    
    public BudgetCategoryService(IBaseRepository<BudgetCategory> baseRepository, IBudgetCategoryRepository budgetCategoryRepository, IMapper mapper) : base(baseRepository)
    {
        _budgetCategoryRepository = budgetCategoryRepository;
        _mapper = mapper;
    }


    public async Task<BudgetCategoryDto> AddAsync(CreateBudgetCategoryDto categoryDto)
    {
        if (categoryDto == null)
            throw new ArgumentNullException(nameof(categoryDto));

        var category = new BudgetCategory
        {
            BudgetCategoryId = Guid.NewGuid(),
            BudgetCategoryName = categoryDto.BudgetCategoryName,
            BudgetId = categoryDto.BudgetId,
            DisplayOrder = categoryDto.DisplayOrder
        };

        await base.AddAsync(category);

        return MapToDto(category);
    }

    public async Task<UpdateBudgetCategoryDto> UpdateAsync(UpdateBudgetCategoryDto categoryDto)
    {
        if (categoryDto == null)
            throw new ArgumentNullException(nameof(categoryDto));

        var category = await GetByIdAsync(categoryDto.BudgetCategoryId);
        if (category == null)
            throw new KeyNotFoundException($"Budget category with ID {categoryDto.BudgetCategoryId} not found.");

        category.BudgetCategoryName = categoryDto.BudgetCategoryName;
        category.DisplayOrder = categoryDto.DisplayOrder;

        await base.UpdateAsync(category);

        return categoryDto;
    }

    public async Task DeleteAsync(Guid categoryId)
    {
        if (categoryId == Guid.Empty)
            throw new ArgumentException("Budget category ID cannot be empty.", nameof(categoryId));

        var category = await GetByIdAsync(categoryId);
        if (category == null)
            throw new KeyNotFoundException($"Budget category with ID {categoryId} not found.");

        await base.DeleteAsync(category);
    }


    public async Task<BudgetCategoryDto?> GetAsync(Guid categoryId)
    {
        if (categoryId == Guid.Empty)
            throw new ArgumentException("Budget category ID cannot be empty.", nameof(categoryId));

        var category = await GetAll()
            .Include(c => c.BudgetSubCategories)
            .FirstOrDefaultAsync(c => c.BudgetCategoryId == categoryId);

        return category == null ? null : MapToDto(category);
    }

    public async Task<List<BudgetCategoryDto>> GetAllByBudgetIdAsync(Guid budgetId)
    {
        if (budgetId == Guid.Empty)
            throw new ArgumentException("Budget ID cannot be empty.", nameof(budgetId));

        var categories = await GetAll()
            .Where(c => c.BudgetId == budgetId)
            .Include(c => c.BudgetSubCategories)
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();

        return categories.Select(MapToDto).ToList();
    }

    #region Private Helper Methods

    /// <summary>
    /// Maps a BudgetCategory entity to a BudgetCategoryDto without subcategories.
    /// </summary>
    /// <param name="category">The budget category entity to map.</param>
    /// <returns>The mapped BudgetCategoryDto.</returns>
    private BudgetCategoryDto MapToDto(BudgetCategory category)
    {
        return _mapper.Map<BudgetCategoryDto>(category);
    }

    #endregion
}
