﻿using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class EllipseDemoPage : ContentPage
    {
        public EllipseDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static EllipseDemoPage Example() => new EllipseDemoPage();
#endif
    }
}
