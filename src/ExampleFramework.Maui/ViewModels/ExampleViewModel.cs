using System.Windows.Input;
using ExampleFramework.Tooling;

namespace ExampleFramework.Maui.ViewModels
{
    public class ExampleViewModel
    {
        public string Title => this.Example.Title;

        public UIExample Example { get; }

        public ICommand TapCommand { get; }

        public ExampleViewModel(UIExample example)
        {
            this.Example = example;

            this.TapCommand = new Command(
                execute: () =>
                {
                    _ = ExamplesViewModel.Instance.ExampleNavigatorService.NavigateToExampleAsync(this.Example).ConfigureAwait(false);
                }
            );
        }
    }
}
