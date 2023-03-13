using System.Collections.Generic;

namespace ExampleBook.Tooling.Maui.ViewModels;

public class UIExamplesManagerDesktopViewModel
{
    private readonly UIExamplesManager _uiExamplesManager;

    public UIExamplesManagerDesktopViewModel(UIExamplesManager uiExamplesManager)
    {
        _uiExamplesManager = uiExamplesManager;
    }

    public IEnumerable<UIExample> AllExamples => _uiExamplesManager.UIExamples.AllExamples;
}
