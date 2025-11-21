using System.ComponentModel.DataAnnotations;
using TheSteward.Core.Models;

namespace TheSteward.Core.DTOs;

public class HouseholdDto
{
    [Key]
    public Guid HouseholdId { get; set; }

    [Required]
    [MaxLength(200)]
    public required string HouseholdName { get; set; }

    [Required]
    public required bool IsHouseholdActive { get; set; }

    [Required]
    public required bool HasTaskManagerAccess { get; set; }

    [Required]
    public required bool HasFinanceManagerAccess { get; set; }

    [Required]
    public required bool HasMealManagerAccess { get; set; }

    [Required]
    public required bool HasFileManagerAccess { get; set; }

    [Required]
    public required string OwnerId { get; set; }

    [Required]
    public required ApplicationUser Owner { get; set; }

    public List<ApplicationUser> Members { get; set; } = new();
}
