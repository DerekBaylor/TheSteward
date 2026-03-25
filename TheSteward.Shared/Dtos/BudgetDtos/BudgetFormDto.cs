namespace TheSteward.Shared.Dtos.BudgetDtos;

public class BudgetFormDto
{
    public Guid? BudgetId { get; set; }
    
    public bool IsEditMode => BudgetId.HasValue;

    public string BudgetName { get; set; } = string.Empty;
    
    public bool IsDefaultBudget { get; set; }

    public bool IsPublic { get; set; }

    public Guid HouseholdId { get; set; }

    public string OwnerId { get; set; } = string.Empty;

    public string CreationType { get; set; } = "blank";
}