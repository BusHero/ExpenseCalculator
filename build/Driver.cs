using System.ComponentModel;
using Nuke.Common.Tooling;

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