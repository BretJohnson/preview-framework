using ExampleFramework.Tooling;
using ExampleFramework.Tooling.Maui.Views;

[assembly: RegisterRemoteControlManager(typeof(RemoteControlManagerMaui))]

namespace ExampleFramework.Tooling;

public class RemoteControlManagerMaui : RemoteControlManager
{
    public override void SetCurrentExample(string componentName, string? exampleName)
    {
        UIComponents components = AppUIExamplesManager.Instance.UIComponents;

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

        if (Application.Current?.MainPage is RemoteControlMainPage remoteControlMainPage)
            remoteControlMainPage.SetExample(example);
    }
}
