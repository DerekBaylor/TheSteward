using System.ComponentModel.DataAnnotations;
using TheSteward.Core.Attributes;
using TheSteward.Core.Models;

namespace TheSteward.Core.Dtos.HouseholdDtos;

public class HouseholdDto
{
    public Guid HouseholdId { get; set; }

    [Required]
    [MaxLength(200)]
    public required string HouseholdName { get; set; }

    public bool IsHouseholdActive { get; set; }

    [HouseholdFeature("Task Manager")]
    public bool HasTaskManagerAccess { get; set; }

    [HouseholdFeature("Finance Manager")]
    public bool HasFinanceManagerAccess { get; set; }

    [HouseholdFeature("Meal Manager")]
    public bool HasMealManagerAccess { get; set; }

    [HouseholdFeature("File Manager")]
    public bool HasFileManagerAccess { get; set; }

    [Required]
    public required string OwnerId { get; set; }

    [Required]
    public required ApplicationUser Owner { get; set; }

    public List<UserHouseholdDto> UserHouseholdDtos { get; set; } = new();
}
