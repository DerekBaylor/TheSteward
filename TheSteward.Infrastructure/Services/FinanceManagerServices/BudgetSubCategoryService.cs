using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TheSteward.Core.Dtos.FinanceManagerDtos;
using TheSteward.Core.IRepositories;
using TheSteward.Core.IRepositories.FinanceManagerIRepositories;
using TheSteward.Core.IServices.FinanceManagerIServices;
using TheSteward.Core.Models.FinanceManagerModels;

namespace TheSteward.Infrastructure.Services.FinanceManagerServices;

public class BudgetSubCategoryService : IBudgetSubCategoryService
{
    private readonly IBudgetSubCategoryRepository _budgetSubCategoryRepository;
    private readonly IMapper _mapper;


    public BudgetSubCategoryService(IBudgetSubCategoryRepository budgetSubCategoryRepository, IMapper mapper)
    {
        _budgetSubCategoryRepository = budgetSubCategoryRepository;
        _mapper = mapper;
    }
    
    public async Task<BudgetSubCategoryDto> AddAsync(CreateBudgetSubCategoryDto subCategoryDto)
    {
        if (subCategoryDto == null)
            throw new ArgumentNullException(nameof(subCategoryDto));

        var subCategory = new BudgetSubCategory
        {
            BudgetSubCategoryId = Guid.NewGuid(),
            BudgetSubCategoryName = subCategoryDto.BudgetSubCategoryName,
            BudgetId = subCategoryDto.BudgetId,
            BudgetCategoryId = subCategoryDto.BudgetCategoryId,
            DisplayOrder = subCategoryDto.DisplayOrder
        };

        await _budgetSubCategoryRepository.AddAsync(subCategory);

        return _mapper.Map<BudgetSubCategoryDto>(subCategory);
    }
    
    public async Task<UpdateBudgetSubCategoryDto> UpdateAsync(UpdateBudgetSubCategoryDto subCategoryDto)
    {
        if (subCategoryDto == null)
            throw new ArgumentNullException(nameof(subCategoryDto));

        var subCategory = await _budgetSubCategoryRepository.GetByIdAsync(subCategoryDto.BudgetSubCategoryId);
        if (subCategory == null)
            throw new KeyNotFoundException($"Budget subcategory with ID {subCategoryDto.BudgetSubCategoryId} not found.");

        subCategory.BudgetSubCategoryName = subCategoryDto.BudgetSubCategoryName;
        subCategory.BudgetCategoryId = subCategoryDto.BudgetCategoryId;
        subCategory.DisplayOrder = subCategoryDto.DisplayOrder;

        await _budgetSubCategoryRepository.UpdateAsync(subCategory);

        return subCategoryDto;
    }
    
    public async Task DeleteAsync(Guid subCategoryId)
    {
        if (subCategoryId == Guid.Empty)
            throw new ArgumentException("Budget subcategory ID cannot be empty.", nameof(subCategoryId));

        var subCategory = await _budgetSubCategoryRepository.GetByIdAsync(subCategoryId);
        if (subCategory == null)
            throw new KeyNotFoundException($"Budget subcategory with ID {subCategoryId} not found.");

        await _budgetSubCategoryRepository.DeleteAsync(subCategory);
    }

    public async Task<BudgetSubCategoryDto?> GetAsync(Guid subCategoryId)
    {
        if (subCategoryId == Guid.Empty)
            throw new ArgumentException("Budget subcategory ID cannot be empty.", nameof(subCategoryId));

        var subCategory = await _budgetSubCategoryRepository.GetByIdAsync(subCategoryId);

        return subCategory == null ? null : _mapper.Map<BudgetSubCategoryDto>(subCategory);;
    }


    public async Task<List<BudgetSubCategoryDto>> GetAllByCategoryIdAsync(Guid categoryId)
    {
        if (categoryId == Guid.Empty)
            throw new ArgumentException("Category ID cannot be empty.", nameof(categoryId));

        var subCategories = await _budgetSubCategoryRepository.GetAll()
            .Where(sc => sc.BudgetCategoryId == categoryId)
            .OrderBy(sc => sc.DisplayOrder)
            .ToListAsync();

        return subCategories.Select(i => _mapper.Map<BudgetSubCategoryDto>(i)).ToList();
    }


    public async Task<List<BudgetSubCategoryDto>> GetAllByBudgetIdAsync(Guid budgetId)
    {
        if (budgetId == Guid.Empty)
            throw new ArgumentException("Budget ID cannot be empty.", nameof(budgetId));

        var subCategories = await _budgetSubCategoryRepository.GetAll()
            .Where(sc => sc.BudgetId == budgetId)
            .OrderBy(sc => sc.DisplayOrder)
            .ToListAsync();

        return subCategories.Select(i => _mapper.Map<BudgetSubCategoryDto>(i)).ToList();
    }
}