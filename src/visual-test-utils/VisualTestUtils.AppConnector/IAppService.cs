namespace VisualTestUtils.AppConnector;

public interface IAppService : IDisposable
{
    Task<string> PingAsync();
    Task<string[]> InvokeMethodAsync(string typeFullName, string staticMethodName, string[] args);
}
