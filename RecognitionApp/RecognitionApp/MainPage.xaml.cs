﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace RecognitionApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        public Library Library = new Library();

        private void New_Click(object sender, RoutedEventArgs e)
        {
            Library.New(Target);
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            Library.Open(Source, Target);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Library.Save(Source, Target);
        }

        private void Sample_Click(object sender, RoutedEventArgs e)
        {
            Library.Sample(Target);
        }
    }
}
