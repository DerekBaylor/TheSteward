

namespace TheSteward.Shared.Models;

public class SelectItem<T>
{
    public T? Value { get; set; }
    public string Text { get; set; } = "";
}
