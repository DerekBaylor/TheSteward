namespace TheSteward.Core.Dtos.HouseholdDtos;

public class InviteUserToHouseholdDto
{
    public Guid HouseholdId { get; set; }
    public string Email { get; set; }
    public bool SetAsDefaultHousehold { get; set; }
}
