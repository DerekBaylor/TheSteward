namespace TheSteward.Core.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class HouseholdFeatureAttribute : Attribute
{
    public string DisplayName { get; set; }
    
    public HouseholdFeatureAttribute(string displayName)
    {
        DisplayName = displayName;
    }
}
