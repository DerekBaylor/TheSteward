namespace TheSteward.Core.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class UserPermissionAttribute : Attribute
{
    public string DisplayName { get; set; }
    public string Category { get; set; }
    public UserPermissionAttribute(string displayName, string category = "")
    {
        DisplayName = displayName;
        Category = category;
    }
}
