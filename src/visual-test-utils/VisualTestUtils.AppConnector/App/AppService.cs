using System.Reflection;

namespace VisualTestUtils.AppConnector.App;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

public class AppService : IAppService
{
    public bool _supportInvoke;

    public AppService(bool supportInvoke = false)
    {
        _supportInvoke = supportInvoke;
    }

    public async Task<string> PingAsync() => "Pong";

    public async Task<string[]> InvokeMethodAsync(string typeFullName, string staticMethodName, string[] args)
    {
        if (!_supportInvoke)
        {
            throw new UnauthorizedAccessException("InvokeMethod supported is not enabled");
        }

        var type = Type.GetType(typeFullName);
        if (type == null)
        {
            throw new InvalidOperationException($"Type {typeFullName} not found");
        }

        MethodInfo? method = type?.GetMethod(staticMethodName, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public);
        if (method == null)
        {
            throw new InvalidOperationException($"Static method {typeFullName}.{staticMethodName} not found");
        }

        object? result = method?.Invoke(null, args);
        return result as string[] ?? Array.Empty<string>();
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
    }
}
