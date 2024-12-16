namespace CoolBro.Domain.Attributes;

public class ActionAttribute : Attribute
{
    public string Action { get; }

    public ActionAttribute(string action)
    {
        Action = action;
    }
}
