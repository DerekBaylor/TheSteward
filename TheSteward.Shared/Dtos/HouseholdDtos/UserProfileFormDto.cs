using System.ComponentModel.DataAnnotations;

namespace TheSteward.Shared.Dtos.HouseholdDtos;

public class UserProfileFormDto
{
    [Required]
    [MaxLength(256)]
    public string UserName { get; set; } = string.Empty;

    [Phone]
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(256)]
    public string Email { get; set; } = string.Empty;
}