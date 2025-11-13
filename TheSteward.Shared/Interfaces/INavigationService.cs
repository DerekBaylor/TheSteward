using Microsoft.AspNetCore.Components;

namespace TheSteward.Shared.Interfaces;

public interface INavigationService
{
    void Initialize(NavigationManager navigation);

    string? CurrentMainSection { get; }

    void SetMainSection(string section);

    event Action? OnChange;

    void Dispose();
}
