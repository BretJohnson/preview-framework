namespace ExampleFramework.Tooling.Maui;

public class ExamplesApp
{
    private static ExamplesApp? _instance;

    public static ExamplesApp Instance
    {
        get
        {
            if (_instance == null)
                throw new InvalidOperationException("ExamplesAppManager isn't initialized");
            return _instance;
        }
    }

    public ExamplesMode ExamplesMode { get; }

    protected ExamplesApp(ExamplesMode examplesAppMode)
    {
        ExamplesMode = examplesAppMode;
    }

    public static void Init(ExamplesApp examplesApp)
    {
        if (_instance != null)
        {
            throw new InvalidOperationException("ExamplesAppManager is already initialized");
        }

        _instance = examplesApp;
    }
}
