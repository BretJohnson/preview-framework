﻿using System;
using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class EditorDemoPage : ContentPage
    {
        public EditorDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static EditorDemoPage Example() => new EditorDemoPage();
#endif
    }
}