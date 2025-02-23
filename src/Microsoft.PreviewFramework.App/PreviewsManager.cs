using System.Reflection;

namespace Microsoft.PreviewFramework.App;

public class PreviewsManager
{
    private readonly static Lazy<PreviewsManager> s_instance = new Lazy<PreviewsManager> (() =>  new PreviewsManager());
    private readonly AppUIComponents _uiComponents;

    public static PreviewsManager Instance => s_instance.Value;

    private PreviewsManager()
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
