using System.ComponentModel;
using Nuke.Common.Tooling;

[TypeConverter(typeof(TypeConverter<Configuration>))]
class Configuration : Enumeration
{
    public static Configuration Debug = new Configuration { Value = nameof(Debug) };
    public static Configuration Release = new Configuration { Value = nameof(Release) };

    public static implicit operator string(Configuration configuration)
    {
        return configuration.Value;
    }
}

[TypeConverter(typeof(TypeConverter<Driver>))]
class Driver : Enumeration
{
    public static Driver Api = new Driver { Value = nameof(Api) };
    
    public static Driver Web = new Driver { Value = nameof(Web) };

    public static implicit operator string(Driver driver)
    {
        return driver.Value;
    }
}
