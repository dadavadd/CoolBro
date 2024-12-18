using System.Text.RegularExpressions;

namespace CoolBro.Domain.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class CallbackDataRegexAttribute : Attribute
{
    public string Pattern { get; }
    public RegexOptions Options { get; }

    public CallbackDataRegexAttribute(string pattern, RegexOptions options = RegexOptions.None)
    {
        Pattern = pattern;
        Options = options;
    }
}
