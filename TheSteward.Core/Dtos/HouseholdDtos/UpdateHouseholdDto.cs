using System.ComponentModel.DataAnnotations;

namespace TheSteward.Core.Dtos.HouseholdDtos;

public class UpdateHouseholdDto
{
    public Guid HouseholdId { get; set; }

    [Required]
    [MaxLength(100)]
    public string HouseholdName { get; set; } = string.Empty;

    public bool IsDefaultHousehold { get; set; } = false;

    public bool HasTaskManagerAccess { get; set; }

    public bool HasFinanceManagerAccess { get; set; }

    public bool HasKitchenManagerAccess { get; set; }

    public bool HasFileManagerAccess { get; set; }
}
