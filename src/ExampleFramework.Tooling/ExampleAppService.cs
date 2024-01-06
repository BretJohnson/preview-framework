using VisualTestUtils;
using VisualTestUtils.AppConnector.App;

namespace ExampleFramework.Tooling;

public abstract class ExampleAppService : AppService, IExampleAppService
{
    public abstract Task NaviateToExampleAsync(string uiComponentName, string? exampleName);
    public abstract Task<ImageSnapshot> GetExampleSnapshotAsync(string uiComponentName, string? exampleName);

    protected static UIComponent GetUIComponent(string uiComponentName)
    {
        UIComponents components = AppUIExamplesManager.Instance.UIComponents;

        UIComponent? component = components.GetUIComponent(uiComponentName);
        if (component == null)
        {
            throw new UIComponentNotFoundException($"UIComponent {uiComponentName} not found");
        }

        return component;
    }

    public Task<string[]> GetUIComponentExamplesAsync(string componentName)
    {
        UIComponent component = GetUIComponent(componentName);
        string[] examples = component.Examples.Select(example => example.FullName).ToArray();

        return Task.FromResult(examples);
    }

    protected static UIExample GetExample(string uiComponentName, string? exampleName)
    {
        UIComponent component = GetUIComponent(uiComponentName);

        UIExample? example;
        if (exampleName != null)
        {
            example = component.GetExample(exampleName);
            if (example == null)
            {
                throw new ExampleNotFoundException($"Example {exampleName} not found for UIComponent {uiComponentName}");
            }
        }
        else
        {
            example = component.GetDefaultExample();
            if (example == null)
            {
                throw new ExampleNotFoundException($"No default example exists for UIComponent {uiComponentName}");
            }
        }

        return example;
    }
}
