using System.Reflection;

namespace ExampleFramework.Tooling;

public class AppUIExamplesManager
{
    private static Lazy<AppUIExamplesManager> _instance = new Lazy<AppUIExamplesManager> (() =>  new AppUIExamplesManager());

    private UIComponents _uiComponents = new UIComponents();

    public static AppUIExamplesManager Instance => _instance.Value;

    private AppUIExamplesManager()
    {
        _uiComponents = new UIComponents();

        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (Assembly assembly in assemblies)
        {
            _uiComponents.AddFromAssembly(assembly);
        }
    }

    public UIComponents UIComponents => _uiComponents;
}
