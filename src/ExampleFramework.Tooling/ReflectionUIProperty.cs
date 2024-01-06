using System.Reflection;

namespace ExampleFramework.Tooling;

public class ReflectionUIProperty : UIProperty
{
    public PropertyInfo PropertyInfo { get; }

    public ReflectionUIProperty(PropertyInfo propertyInfo) : base(propertyInfo.Name)
    {
        this.PropertyInfo = propertyInfo;
    }

    public override object GetValue(object obj) => this.PropertyInfo.GetValue(obj);

    public override void SetValue(object obj, object propertyValue) => this.PropertyInfo.SetValue(obj, propertyValue);
}
