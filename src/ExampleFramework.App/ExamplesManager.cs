using System.Reflection;

namespace ExampleFramework.App;

public class ExamplesManager
{
    private static Lazy<ExamplesManager> instance = new Lazy<ExamplesManager> (() =>  new ExamplesManager());
    private readonly AppUIComponents uiComponents;

    public static ExamplesManager Instance => instance.Value;

    private ExamplesManager()
    {
        this.uiComponents = new AppUIComponents();

        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (Assembly assembly in assemblies)
        {
            this.uiComponents.AddFromAssembly(assembly);
        }
    }

    public AppUIComponents UIComponents => this.uiComponents;
}
