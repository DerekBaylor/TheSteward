using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using TheSteward.Shared.Interfaces;

namespace TheSteward.Shared.Services;

public class NavigationService : INavigationService
{
    public string? CurrentMainSection { get; private set; }

    public event Action? OnChange;

    private NavigationManager? Navigation { get; set; }

    public void Dispose()
    {
        if (Navigation != null)
            Navigation.LocationChanged -= HandleLocationChanged;
    }

    private void Initialize(NavigationManager navigation)
    {
        Navigation = navigation;
        Navigation.LocationChanged += HandleLocationChanged;
        ParseCurrentUrl();
    }

    public void SetMainSection(string section)
    {
        CurrentMainSection = section;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();

    private void HandleLocationChanged(object? sender, LocationChangedEventArgs eventArgs)
    {
        ParseCurrentUrl();
        NotifyStateChanged();
    }

    private void ParseCurrentUrl()
    {
        if (Navigation == null) return;

        var uri = new Uri(Navigation.Uri);
        var segments = uri.Segments;
    }

    void INavigationService.Initialize(NavigationManager navigation)
    {
        Initialize(navigation);
    }

}
