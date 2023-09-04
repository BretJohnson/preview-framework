namespace ExampleFramework.Tooling;

public class UIComponentCategory
{
    private string _name;

    public UIComponentCategory(string name)
    {
        _name = name;
    }

    public string Name => _name;
}
