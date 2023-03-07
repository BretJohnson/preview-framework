using ExampleBook.Tooling;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ExampleBook;

public class Examples
{
    private readonly List<Example> _examples = new();

    public void LoadFromAssembly(Assembly assembly)
    {
        Type[] types = assembly.GetExportedTypes();

        foreach (Type type in types)
        {
            MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly);

            foreach (MethodInfo method in methods)
            {
                ExampleAttribute? uiExampleAttribute = method.GetCustomAttribute<ExampleAttribute>(false);

                if (uiExampleAttribute != null)
                {
                    var uiExample = new Example(uiExampleAttribute, method);
                    _examples.Add(uiExample);
                }
            }
        }
    }

    public IEnumerable<Example> AllExamples => _examples;
}
