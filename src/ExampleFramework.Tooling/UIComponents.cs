using System.Reflection;

namespace ExampleFramework.Tooling;

public class UIComponents
{
    private readonly Dictionary<string, UIComponent> _uiComponentsCollection = new();

    public void AddFromAssembly(Assembly assembly)
    {
        Type[] types = assembly.GetExportedTypes();

        foreach (Type type in types)
        {
            UIExampleAttribute? typeExampleAttribute = type.GetCustomAttribute<UIExampleAttribute>(false);
            if (typeExampleAttribute != null)
            {
                AddExample(new ClassUIExample(typeExampleAttribute, type));
            }

            MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly);
            foreach (MethodInfo method in methods)
            {
                UIExampleAttribute? uiExampleAttribute = method.GetCustomAttribute<UIExampleAttribute>(false);

                if (uiExampleAttribute != null)
                {
                    AddExample(new StaticMethodUIExample(uiExampleAttribute, method));
                }
            }
        }
    }

    public IEnumerable<UIComponent> UIComponentsCollection => _uiComponentsCollection.Values;

    public UIComponent? GetUIComponent(string name) =>
        _uiComponentsCollection.TryGetValue(name, out UIComponent? uiComponent) ? uiComponent : null;

    public UIComponent GetOrAddUIComponent(Type type)
    {
        string name = type.FullName;

        if (!_uiComponentsCollection.TryGetValue(name, out UIComponent? uiComponent))
        {
            uiComponent = new UIComponent(null, type);
            _uiComponentsCollection.Add(name, uiComponent);
        }

        return uiComponent;
    }

    public void AddExample(UIExample uiExample)
    {
        UIComponent uiComponent = GetOrAddUIComponent(uiExample.UIComponentType);
        uiComponent.AddExample(uiExample);
    }
}
