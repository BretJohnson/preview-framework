namespace ExampleFramework.Tooling;

public abstract class RemoteControlManager : IRemoteControlManager
{
    public abstract void SetCurrentExample(string componentName, string? exampleName);

    public void SetCurrentExampleHelper(string componentName, string? exampleName)
    {
        UIComponents components = UIExamplesManager.Instance.UIComponents;

        UIComponent? component = components.GetComponent(componentName);
        if (component == null)
        {
            return;
        }

        UIExample? example;
        if (exampleName != null)
        {
            example = component.GetExample(exampleName);
        }
        else
        {
            example = component.GetDefaultExample();
        }
        if (example == null)
            return;

    }
}