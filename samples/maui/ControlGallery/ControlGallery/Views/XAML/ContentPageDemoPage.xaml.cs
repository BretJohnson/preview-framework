﻿using System;
using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class ContentPageDemoPage : ContentPage
    {
        public ContentPageDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static ContentPageDemoPage Example() => new ContentPageDemoPage();
#endif
    }
}