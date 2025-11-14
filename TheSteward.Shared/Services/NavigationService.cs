using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using TheSteward.Shared.Interfaces;

namespace TheSteward.Shared.Services;

public class NavigationService : INavigationService, IDisposable
{
    private bool _isInitialized;
    public string? CurrentMainSection { get; private set; }

    public event Action? OnChange;

    private NavigationManager? _navigation { get; set; }

    public void Dispose()
    {
        if (_navigation != null)
            _navigation.LocationChanged -= HandleLocationChanged;
    }

    public void Initialize(NavigationManager navigation)
    {
        if (_isInitialized) return;

        _navigation = navigation;
        _navigation.LocationChanged += HandleLocationChanged;
        _isInitialized = true;

        ParseCurrentUrl();
    }

    public void SetMainSection(string section)
    {
        if (CurrentMainSection != section)
        {
            CurrentMainSection = section;
            NotifyStateChanged();
        }
    }

    private void NotifyStateChanged() => OnChange?.Invoke();

    private void HandleLocationChanged(object? sender, LocationChangedEventArgs eventArgs)
    {
        ParseCurrentUrl();
        NotifyStateChanged();
    }

    private void ParseCurrentUrl()
    {
        if (_navigation == null) return;

        var uri = new Uri(_navigation.Uri);
        var path = uri.AbsolutePath.TrimStart('/');

        var firstSegment = path.Split('/', StringSplitOptions.RemoveEmptyEntries)
                                .FirstOrDefault();

        CurrentMainSection = string.IsNullOrEmpty(firstSegment)
            ? "Dashboard"
            : char.ToUpper(firstSegment[0]) + firstSegment.Substring(1);

        NotifyStateChanged();
    }

    void INavigationService.Initialize(NavigationManager navigation)
    {
        Initialize(navigation);
    }

}
