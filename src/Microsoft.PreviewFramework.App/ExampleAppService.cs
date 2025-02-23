using VisualTestUtils;
using VisualTestUtils.AppConnector.App;

namespace Microsoft.PreviewFramework.App;

public abstract class ExampleAppService : AppService, IExampleAppService
{
    public abstract Task NavigateToExampleAsync(string uiComponentName, string exampleName);
    public abstract Task<ImageSnapshot> GetExampleSnapshotAsync(string uiComponentName, string exampleName);

    protected static AppUIComponent GetUIComponent(string uiComponentName)
    {
        AppUIComponents uiComponents = ExamplesManager.Instance.UIComponents;

        AppUIComponent? uiComponent = uiComponents.GetUIComponent(uiComponentName);
        if (uiComponent == null)
        {
            throw new UIComponentNotFoundException($"UIComponent {uiComponentName} not found");
        }

        return uiComponent;
    }

    public Task<string[]> GetUIComponentExamplesAsync(string componentName)
    {
        AppUIComponent component = GetUIComponent(componentName);
        string[] exampleNames = component.Examples.Select(example => example.Name).ToArray();

        return Task.FromResult(exampleNames);
    }

    protected static AppUIExample GetExample(string uiComponentName, string exampleName)
    {
        AppUIComponent uiComponent = GetUIComponent(uiComponentName);

        AppUIExample? example = uiComponent.GetExample(exampleName);
        if (example == null)
        {
            throw new ExampleNotFoundException($"Example {exampleName} not found for UIComponent {uiComponentName}");
        }

        return example;
    }
}
