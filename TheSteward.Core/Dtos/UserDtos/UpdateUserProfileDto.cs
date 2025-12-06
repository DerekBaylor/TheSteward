using System.ComponentModel.DataAnnotations;

namespace TheSteward.Core.Dtos.UserDtos;

public class UpdateUserProfileDto
{
    [Required]
    public required string UserId { get; set; }

    [Required]
    [MaxLength(256)]
    public required string UserName { get; set; }

    [Phone]
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(256)]
    public required string Email { get; set; }
}
