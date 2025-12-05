using System.ComponentModel.DataAnnotations;

namespace TheSteward.Core.Models.HouseholdModels;

public class HouseholdInvitation
{
    [Key]
    public Guid InvitationId { get; set; }

    [Required]
    public required Guid HouseholdId { get; set; }

    [Required]
    public required Household Household { get; set; }

    [Required]
    [EmailAddress]
    public required string InvitedUserEmail { get; set; }

    [Required]
    public required string InvitedByUserId { get; set; }

    [Required]
    public required ApplicationUser InvitedByUser { get; set; }

    [Required]
    public required DateTime InvitedDate { get; set; }

    public DateTime? ExpirationDate { get; set; }

    [Required]
    public required bool IsAccepted { get; set; }

    public DateTime? AcceptedDate { get; set; }
}