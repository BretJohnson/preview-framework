using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace ExampleBook.Tooling;

public class UIExamples
{
    private readonly List<UIExample> _examples = new();

    public void AddFromAssembly(Assembly assembly)
    {
        Type[] types = assembly.GetExportedTypes();

        foreach (Type type in types)
        {
            MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly);

            foreach (MethodInfo method in methods)
            {
                UIExampleAttribute? uiExampleAttribute = method.GetCustomAttribute<UIExampleAttribute>(false);

                if (uiExampleAttribute != null)
                {
                    var uiExample = new UIExample(uiExampleAttribute, method);
                    _examples.Add(uiExample);
                }
            }
        }
    }

    public IEnumerable CurrentExamples => _examples;

    public IEnumerable<UIExample> AllExamples => _examples;
}
