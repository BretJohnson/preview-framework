using System.Collections;
using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class FlyoutPageDemoPage : FlyoutPage
    {
        public FlyoutPageDemoPage()
        {
            InitializeComponent();

            listView.SelectedItem = (listView.ItemsSource as IList)?[0];
        }

#if EXAMPLES
        [UIExample("Example")]
        public static FlyoutPageDemoPage Example() => new FlyoutPageDemoPage();
#endif

        void OnListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            // Show the detail page.
            IsPresented = false;
        }
    }
}