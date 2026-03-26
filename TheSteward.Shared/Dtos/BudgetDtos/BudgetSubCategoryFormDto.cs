namespace TheSteward.Shared.Dtos.BudgetDtos;

public class BudgetSubCategoryFormDto
{
    public Guid? BudgetSubCategoryId { get; set; }
    public string BudgetSubCategoryName { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public Guid BudgetId { get; set; }
    public Guid BudgetCategoryId { get; set; }
    public bool IsEditing { get; set; } = false;
    public bool IsNew { get; set; } = false;
}
