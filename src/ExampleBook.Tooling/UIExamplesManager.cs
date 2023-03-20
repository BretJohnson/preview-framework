using System;
using System.Reflection;

namespace ExampleBook.Tooling;

public class UIExamplesManager
{
    private static Lazy<UIExamplesManager> _instance = new Lazy<UIExamplesManager> (() =>  new UIExamplesManager());

    private UIComponents _uiComponents = new UIComponents();

    public static UIExamplesManager Instance => _instance.Value;

    private UIExamplesManager()
    {
        _uiComponents = new UIComponents();

        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (Assembly assembly in assemblies)
        {
            _uiComponents.AddFromAssembly(assembly);
        }
    }

    public UIComponents UIComponents => _uiComponents;

    public void SetCurrentExample(string componentName, string? exampleName)
    {
        UIComponent? component = _uiComponents.GetComponentWithName(componentName);
        if (component == null)
            return;

        UIExample? example;
        if (exampleName != null)
        {
            example = component.GetExampleWithName(exampleName);
        }
        else
        {
            example = component.GetDefaultExample();
        }
        if (example == null)
            return;


    }
}