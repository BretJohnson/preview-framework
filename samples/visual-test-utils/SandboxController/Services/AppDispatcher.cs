using Drastic.Services;

namespace SandboxServer.Services;

public class AppDispatcher : IAppDispatcher
{
    public bool Dispatch(Action action)
    {
        action.Invoke();
        return true;
    }
}
