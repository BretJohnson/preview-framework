using Microsoft.CodeAnalysis;

namespace Microsoft.PreviewFramework.Tooling;

public class PreviewsManager
{
    private ToolingUIComponents uiComponents = new ToolingUIComponents();

    public PreviewsManager(Compilation compilation)
    {
        this.uiComponents = new ToolingUIComponents();

#if false
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (Assembly assembly in assemblies)
        {
            _uiComponents.AddFromAssembly(assembly);
        }
#endif
    }

    public ToolingUIComponents UIComponents => this.uiComponents;
}
