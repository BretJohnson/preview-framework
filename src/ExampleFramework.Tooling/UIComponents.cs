using System.Reflection;

namespace ExampleFramework.Tooling;

public class UIComponents
{
    private readonly Dictionary<string, UIComponentCategory> _categories = new();
    private readonly Dictionary<string, UIComponent> _components = new();

    public void AddFromAssembly(Assembly assembly)
    {
        IEnumerable<UIComponentCategoryAttribute> uiComponentCategoryAttributes = assembly.GetCustomAttributes<UIComponentCategoryAttribute>();
        foreach (UIComponentCategoryAttribute uiComponentCategoryAttribute in uiComponentCategoryAttributes)
        {
            UIComponentCategory category = GetOrAddCatgegory(uiComponentCategoryAttribute.Name);

            foreach (Type type in uiComponentCategoryAttribute.UIComponentTypes)
            {
                UIComponent component = GetOrAddUIComponent(type);
                component.SetCategoryFailIfAlreadySet(category);
            }
        }

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

    public IEnumerable<UIComponentCategory> Categories => _categories.Values;

    public IEnumerable<UIComponent> Components => _components.Values;

    public UIComponent? GetUIComponent(string name) =>
        _components.TryGetValue(name, out UIComponent? uiComponent) ? uiComponent : null;

    public UIComponentCategory GetOrAddCatgegory(string name)
    {
        if (!_categories.TryGetValue(name, out UIComponentCategory? category))
        {
            category = new UIComponentCategory(name);
            _categories.Add(name, category);
        }

        return category;
    }

    public UIComponent GetOrAddUIComponent(Type type)
    {
        string name = type.FullName;

        if (!_components.TryGetValue(name, out UIComponent? uiComponent))
        {
            uiComponent = new UIComponent(null, type);
            _components.Add(name, uiComponent);
        }

        return uiComponent;
    }

    public void AddExample(UIExample uiExample)
    {
        UIComponent uiComponent = GetOrAddUIComponent(uiExample.UIComponentType);
        uiComponent.AddExample(uiExample);
    }
}
