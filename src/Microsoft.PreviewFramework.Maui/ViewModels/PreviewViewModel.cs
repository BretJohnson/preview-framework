using System.Windows.Input;
using Microsoft.PreviewFramework.App;

namespace Microsoft.PreviewFramework.Maui.ViewModels
{
    public class PreviewViewModel
    {
        public string Title => this.Preview.DisplayName;

        public AppPreview Preview { get; }

        public ICommand TapCommand { get; }

        public PreviewViewModel(AppPreview preview)
        {
            this.Preview = preview;

            this.TapCommand = new Command(
                execute: () =>
                {
                    _ = PreviewsViewModel.Instance.PreviewNavigatorService.NavigateToPreviewAsync(this.Preview).ConfigureAwait(false);
                }
            );
        }
    }
}
