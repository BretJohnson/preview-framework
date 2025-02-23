namespace Microsoft.PreviewFramework.App;

public interface IExampleNavigatorService
{
    public Task NavigateToExampleAsync(AppUIExample example);
}
