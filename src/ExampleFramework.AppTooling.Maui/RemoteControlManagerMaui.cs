using ExampleFramework.Tooling;
using ExampleFramework.Tooling.Maui.Views;

[assembly: RegisterRemoteControlManager(typeof(RemoteControlManagerMaui))]

namespace ExampleFramework.Tooling;

public class RemoteControlManagerMaui : RemoteControlManager
{
    public override void SetCurrentExample(string componentName, string? exampleName)
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

        var app = Application.Current;
        if (Application.Current?.MainPage is RemoteControlMainPage remoteControlMainPage)
            remoteControlMainPage.SetExample(example);
    }
}