using System.Reflection;

namespace ExampleFramework.Tooling;

public class ReflectionUIProperty : UIProperty
{
    public PropertyInfo PropertyInfo { get; }

    public ReflectionUIProperty(PropertyInfo propertyInfo) : base(propertyInfo.Name)
    {
        PropertyInfo = propertyInfo;
    }

    public override object GetValue(object obj) => PropertyInfo.GetValue(obj);

    public override void SetValue(object obj, object propertyValue) => PropertyInfo.SetValue(obj, propertyValue);
}
