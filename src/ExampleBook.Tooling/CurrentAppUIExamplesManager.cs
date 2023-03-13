using System;
using System.Reflection;

namespace ExampleBook.Tooling.Maui;

public class CurrentAppUIExamplesManager : UIExamplesManager
{
    private static Lazy<CurrentAppUIExamplesManager> _instance = new Lazy<CurrentAppUIExamplesManager> (() =>  new CurrentAppUIExamplesManager ());

    private UIExamples _uiExamples = new UIExamples();

    public static CurrentAppUIExamplesManager Instance => _instance.Value;

    private CurrentAppUIExamplesManager()
    {
        _uiExamples = new UIExamples();

        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (Assembly assembly in assemblies)
        {
            _uiExamples.AddFromAssembly(assembly);
        }
    }

    public override UIExamples UIExamples => _uiExamples;
}