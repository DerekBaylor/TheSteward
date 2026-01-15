using AutoMapper;
using TheSteward.Core.Dtos.FinanceManagerDtos;
using TheSteward.Core.Models.FinanceManagerModels;

namespace TheSteward.Core.Profiles;

public class FinanceManagerProfiles : Profile
{
    public FinanceManagerProfiles()
    {
        CreateMap<Budget, BudgetDto>()
        .ReverseMap();

        CreateMap<BudgetCategory, BudgetCategoryDto>()
            .ForMember(dest => dest.BudgetSubCategories,
                opt => opt.MapFrom(src => src.BudgetSubCategories ?? new List<BudgetSubCategory>()))
            .ReverseMap();
        
        CreateMap<BudgetSubCategory, BudgetSubCategoryDto>()
            .ReverseMap();
        
        CreateMap<Credit, CreditDto>()
            .ReverseMap();
        
        CreateMap<Expense, ExpenseDto>()
            .ReverseMap();
        
        CreateMap<Income, IncomeDto>()
            .ReverseMap();
        
        
        CreateMap<Investment, InvestmentDto>()
            .ReverseMap();
        
    }
}