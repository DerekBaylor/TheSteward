namespace TheSteward.Core.Dtos.HouseholdDtos;

public class HouseholdInvitationDto
{
    public Guid InvitationId { get; set; }
    public required Guid HouseholdId { get; set; }
    public required string HouseholdName { get; set; }
    public required string InvitedByUserName { get; set; }
    public required string InvitedUserEmail { get; set; }
    public DateTime InvitedDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public bool IsAccepted { get; set; }
}