using System.ComponentModel.DataAnnotations;

namespace TheSteward.Shared.DTOs;

public class HouseholdFormDto
{
    [Required]
    [MaxLength(100)]
    public string HouseholdName { get; set; } = string.Empty;

    public bool HasTaskManagerAccess { get; set; }
    public bool HasFinanceManagerAccess { get; set; }
    public bool HasMealManagerAccess { get; set; }
    public bool HasFileManagerAccess { get; set; }
}
