using TheSteward.Core.Dtos.HouseholdDtos;
using TheSteward.Shared.Dtos.HouseholdDtos;

public class HouseholdState
{
    public UserHouseholdDto? CurrentUserHousehold { get; private set; }
    public HouseholdDto? CurrentHousehold => CurrentUserHousehold?.Household;
    public bool HasAdminPermissions => CurrentUserHousehold?.HasAdminPermissions ?? false;
    public bool HasFinanceReadPermission => CurrentUserHousehold?.HasFinanceManagerReadPermission ?? false;
    public bool HasFinanceWritePermission => CurrentUserHousehold?.HasFinanceManagerWritePermission ?? false;

    public event Action? OnChange;

    public void SetUserHousehold(UserHouseholdDto userHousehold)
    {
        CurrentUserHousehold = userHousehold;
        NotifyStateChanged();
    }

    public void ClearHousehold()
    {
        CurrentUserHousehold = null;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}