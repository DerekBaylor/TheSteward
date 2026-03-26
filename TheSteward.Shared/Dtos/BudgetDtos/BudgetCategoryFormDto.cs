namespace TheSteward.Shared.Dtos.BudgetDtos;

public class BudgetCategoryFormDto
{
    public Guid? BudgetCategoryId { get; set; }
    public string BudgetCategoryName { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public Guid BudgetId { get; set; }
    public bool IsEditing { get; set; } = false;
    public bool IsNew { get; set; } = false;
}

