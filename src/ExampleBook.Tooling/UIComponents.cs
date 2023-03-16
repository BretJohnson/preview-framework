using System;
using System.Collections.Generic;
using System.Reflection;

namespace ExampleBook.Tooling;

public class UIComponents
{
    private readonly List<UIComponent> _components = new();

    public void AddFromAssembly(Assembly assembly)
    {
        Type[] types = assembly.GetExportedTypes();

        foreach (Type type in types)
        {
            MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly);

            UIComponent? component = null;
            foreach (MethodInfo method in methods)
            {
                UIExampleAttribute? uiExampleAttribute = method.GetCustomAttribute<UIExampleAttribute>(false);

                if (uiExampleAttribute != null)
                {
                    if (component == null)
                    {
                        component = new UIComponent(null, method.ReturnType);
                        _components.Add(component);
                    }

                    var uiExample = new UIExample(uiExampleAttribute, method);
                    component.AddExample(uiExample);
                }
            }
        }
    }

    public IEnumerable<UIComponent> Components => _components;
}
