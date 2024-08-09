using ExampleFramework.Maui.Pages;

namespace ExampleFramework.Maui;

public class MauiExamplesApplication
{
    public static MauiExamplesApplication Instance
    {
        get
        {
            if (MauiExamplesApplication.instance == null)
                throw new InvalidOperationException("MauiExamplesManager.Init hasn't been called");
            else return instance;
        }
    }

    private static MauiExamplesApplication? instance;

    private bool navigatingToExample = false;
    private int savedNavigationStackCount = 0;

    public Application Application { get; }

    internal static void Init(Application application)
    {
        if (instance != null)
            throw new InvalidOperationException("MauiExamplesManager.Init has already been called");

        instance = new MauiExamplesApplication(application);
    }

    private MauiExamplesApplication(Application application)
    {
        this.Application = application;
    }

    public async Task ShowExamplesAsync()
    {
        if (this.navigatingToExample)
        {
            // The user may navigate around while inside an example. If they do that, pop the navigation
            // stack back to where it was before they navigated to the example.
            int currentNavigationStackCount = this.Application.MainPage!.Navigation.NavigationStack.Count;
            if (currentNavigationStackCount > this.savedNavigationStackCount)
            {
                int amountToPop = currentNavigationStackCount - this.savedNavigationStackCount;
                for (int i = 0; i < amountToPop; i++)
                {
                    _ = this.Application.MainPage!.Navigation.PopAsync();
                }
            }

            this.navigatingToExample = false;
            this.savedNavigationStackCount = 0;
        }

        await this.Application.MainPage!.Navigation.PushModalAsync(new ExamplesPage());
    }

    public void NavigateToPageAsync(Page page)
    {
        _ = this.Application.MainPage!.Navigation.PopModalAsync();
    }

    public void PrepareToNavigateToExample()
    {
        this.navigatingToExample = true;
        this.savedNavigationStackCount = this.Application.MainPage!.Navigation.NavigationStack.Count - 1;
        _ = this.Application.MainPage!.Navigation.PopModalAsync();
    }
}
