using TheSteward.Core.Dtos.HouseholdDtos;

public class HouseholdState
{
    public HouseholdDto? CurrentHousehold { get; private set; }

    public event Action? OnChange;

    public void SetHousehold(HouseholdDto household)
    {
        CurrentHousehold = household;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
