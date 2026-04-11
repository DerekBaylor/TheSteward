using TheSteward.Core.Dtos.HouseholdDtos;
using TheSteward.Shared.Dtos.HouseholdDtos;

public class HouseholdState
{
    #region Properties

    public UserHouseholdDto? CurrentUserHousehold { get; private set; }
    public List<UserHouseholdDto> UserHouseholds { get; private set; } = new();
    public HouseholdDto? CurrentHousehold => CurrentUserHousehold?.Household;

    #endregion Properties

    #region Permissions

    public bool HasAdminPermissions => CurrentUserHousehold?.HasAdminPermissions ?? false;

    public bool HasFinanceReadPermission => CurrentUserHousehold?.HasFinanceManagerReadPermission ?? false;
    public bool HasFinanceWritePermission => CurrentUserHousehold?.HasFinanceManagerWritePermission ?? false;

    public bool HasTaskManagerReadPermission => CurrentUserHousehold?.HasTaskManagerReadPermission ?? false;
    public bool HasTaskManagerWritePermission => CurrentUserHousehold?.HasTaskManagerWritePermission ?? false;
    public bool HasTaskManagerCompletePermission => CurrentUserHousehold?.HasTaskManagerCompletePermission ?? false;
    #endregion Permissions

    public event Action? OnChange;

    public void SetUserHousehold(UserHouseholdDto userHousehold)
    {
        CurrentUserHousehold = userHousehold;
        NotifyStateChanged();
    }

    public void SetUserHouseholds(List<UserHouseholdDto> households)
    {
        UserHouseholds = households;
        CurrentUserHousehold = households.FirstOrDefault(uh => uh.IsDefaultUserHousehold)
                               ?? households.FirstOrDefault();
        NotifyStateChanged();
    }

    public void ClearHousehold()
    {
        CurrentUserHousehold = null;
        UserHouseholds = new();
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}


