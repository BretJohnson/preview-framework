using System;
using System.Reflection;

namespace ExampleBook.Tooling.Maui;

public class CurrentAppUIExamplesManager : UIExamplesManager
{
    private static Lazy<CurrentAppUIExamplesManager> _instance = new Lazy<CurrentAppUIExamplesManager> (() =>  new CurrentAppUIExamplesManager ());

    private UIComponents _uiComponents = new UIComponents();

    public static CurrentAppUIExamplesManager Instance => _instance.Value;

    private CurrentAppUIExamplesManager()
    {
        _uiComponents = new UIComponents();

        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (Assembly assembly in assemblies)
        {
            _uiComponents.AddFromAssembly(assembly);
        }
    }

    public override UIComponents UIComponents => _uiComponents;
}