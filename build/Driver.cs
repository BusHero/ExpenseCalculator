using System.ComponentModel;
using Nuke.Common.Tooling;

[TypeConverter(typeof(TypeConverter<Driver>))]
sealed class Driver : Enumeration
{
    public readonly static Driver Api = new()
    {
        Value = nameof(Api),
    };

    public readonly static Driver Web = new()
    {
        Value = nameof(Web),
    };

    public static implicit operator string(Driver driver) 
        => driver.Value;
}
