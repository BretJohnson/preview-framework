namespace ExampleFramework.Tooling;

public abstract class UIProperty
{
    private readonly string _name;

    public UIProperty(string name)
    {
        _name = name;
    }

    public abstract object GetValue(object obj);
    public abstract void SetValue(object obj, object propertyValue);
}
