namespace ExampleFramework.Tooling.Maui;

public static class MauiAppBuilderExtensions
{
    public static void EnableExamplesMode<TApp>(this MauiAppBuilder builder) where TApp : class, IApplication
    {
        MauiAppManager.EnableExamplesMode<TApp>(builder);
    }
}
