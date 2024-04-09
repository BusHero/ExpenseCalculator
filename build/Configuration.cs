using System.ComponentModel;
using Nuke.Common.Tooling;

[TypeConverter(typeof(TypeConverter<Configuration>))]
sealed class Configuration : Enumeration
{
    public readonly static Configuration Debug = new()
    {
        Value = nameof(Debug),
    };
    public readonly static Configuration Release = new()
    {
        Value = nameof(Release),
    };

    public static implicit operator string(Configuration configuration) 
        => configuration.Value;
}
