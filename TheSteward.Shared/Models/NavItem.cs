namespace TheSteward.Shared.Models;

public record NavItem(
    string Title,
    string MaterialIcon,
    string Route,
    bool RequiresAuth,
    bool IsLogout
);