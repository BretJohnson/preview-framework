using System.Reflection;

namespace ExampleFramework.App;

public class ExamplesManager
{
    private readonly static Lazy<ExamplesManager> s_instance = new Lazy<ExamplesManager> (() =>  new ExamplesManager());
    private readonly AppUIComponents _uiComponents;

    public static ExamplesManager Instance => s_instance.Value;

    private ExamplesManager()
    {
        this._uiComponents = new AppUIComponents();

        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (Assembly assembly in assemblies)
        {
            _uiComponents.AddFromAssembly(assembly);
        }
    }

    public AppUIComponents UIComponents => _uiComponents;
}
