using System;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace GaugeControl
{
    public sealed partial class Gauge : UserControl
    {
        public Gauge()
        {
            this.InitializeComponent();
            init(Display);
        }

        private Brush foreground = new SolidColorBrush(Windows.UI.Colors.White);
        private Brush background = ((Brush)App.Current.Resources["SystemControlBackgroundAccentBrush"]);
        private Windows.UI.Xaml.Shapes.Rectangle needle;
        private int needleWidth = 2;
        private int needleLength = 0;
        private double diameter = 0;
        private int value = 0;
        private int minimum = 0;
        private int maximum = 100;

        private TransformGroup transformGroup(double angle, double x, double y)
        {
            TransformGroup transformGroup = new TransformGroup();
            TranslateTransform firstTranslate = new TranslateTransform()
            {
                X = x,
                Y = y
            };
            transformGroup.Children.Add(firstTranslate);
            RotateTransform rotateTransform = new RotateTransform()
            {
                Angle = angle
            };
            transformGroup.Children.Add(rotateTransform);
            TranslateTransform secondTranslate = new TranslateTransform()
            {
                X = diameter / 2,
                Y = diameter / 2
            };
            transformGroup.Children.Add(secondTranslate);
            return transformGroup;
        }

        private void indicator(int value)
        {
            double percentage = (((double)value / (double)maximum) * 100);
            double position = (percentage / 2) + 5;
            needle.RenderTransform = transformGroup(position * 6,
            -needleWidth / 2, -needleLength + 4.25);
        }

        private void init(Canvas canvas)
        {
            canvas.Children.Clear();
            diameter = canvas.Width;
            double inner = diameter;
            Windows.UI.Xaml.Shapes.Ellipse face = new Windows.UI.Xaml.Shapes.Ellipse()
            {
                Height = diameter,
                Width = diameter,
                Fill = background
            };
            canvas.Children.Add(face);
            Canvas markers = new Canvas()
            {
                Width = inner,
                Height = inner
            };
            for (int i = 0; i < 51; i++)
            {
                Windows.UI.Xaml.Shapes.Rectangle marker = new Windows.UI.Xaml.Shapes.Rectangle()
                {
                    Fill = foreground
                };
                if ((i % 5) == 0)
                {
                    marker.Width = 4;
                    marker.Height = 16;
                    marker.RenderTransform = transformGroup(i * 6, -(marker.Width / 2),
                    -(marker.Height * 2 + 4.5 - face.StrokeThickness / 2 - inner / 2 - 16));
                }
                else
                {
                    marker.Width = 2;
                    marker.Height = 8;
                    marker.RenderTransform = transformGroup(i * 6, -(marker.Width / 2),
                    -(marker.Height * 2 + 12.75 - face.StrokeThickness / 2 - inner / 2 - 16));
                }
                markers.Children.Add(marker);
            }
            markers.RenderTransform = new RotateTransform()
            {
                Angle = 30,
                CenterX = diameter / 2,
                CenterY = diameter / 2
            };
            canvas.Children.Add(markers);
            needle = new Windows.UI.Xaml.Shapes.Rectangle()
            {
                Width = needleWidth,
                Height = (int)diameter / 2 - 30,
                Fill = foreground
            };
            canvas.Children.Add(needle);
            Windows.UI.Xaml.Shapes.Ellipse middle = new Windows.UI.Xaml.Shapes.Ellipse()
            {
                Height = diameter / 10,
                Width = diameter / 10,
                Fill = foreground
            };
            Canvas.SetLeft(middle, (diameter - middle.ActualWidth) / 2);
            Canvas.SetTop(middle, (diameter - middle.ActualHeight) / 2);
            canvas.Children.Add(middle);
        }

        public int Minimum { get { return minimum; } set { minimum = value; } }

        public int Maximum { get { return maximum; } set { maximum = value; } }

        public int Value
        {
            get
            {
                return value;
            }
            set
            {
                if (value >= Minimum && value <= Maximum)
                {
                    this.value = value;
                    indicator(value);
                }
            }
        }
    }
}
