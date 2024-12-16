namespace CoolBro.Domain.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class CallbackDataAttribute : Attribute
{
    public string Command { get; }

    public CallbackDataAttribute(string command)
    {
        Command = command;
    }
}
