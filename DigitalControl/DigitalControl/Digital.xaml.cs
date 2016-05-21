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

namespace DigitalControl
{
    public sealed partial class Digital : UserControl
    {
        public Digital()
        {
            this.InitializeComponent();
        }

        private int[][] table =
        {
            // a, b, c, d, e, f, g
            new int[] {  1, 1, 1, 1, 1, 1, 0 }, // 0
            new int[] {  0, 1, 1, 0, 0, 0, 0 }, // 1
            new int[] {  1, 1, 0, 1, 1, 0, 1 }, // 2
            new int[] {  1, 1, 1, 1, 0, 0, 1 }, // 3
            new int[] {  0, 1, 1, 0, 0, 1, 1 }, // 4
            new int[] {  1, 0, 1, 1, 0, 1, 1 }, // 5
            new int[] {  1, 0, 1, 1, 1, 1, 1 }, // 6
            new int[] {  1, 1, 1, 0, 0, 0, 0 }, // 7
            new int[] {  1, 1, 1, 1, 1, 1, 1 }, // 8
            new int[] {  1, 1, 1, 0, 0, 1, 1 }, // 9
            new int[] {  0, 0, 0, 0, 0, 0, 0 }, // None
            new int[] {  0, 0, 0, 0, 0, 0, 0 }, // Colon
        };

        private int _digit;

        public int Digit
        {
            get { return _digit; }
            set
            {
                _digit = value;
                int[] values = table[_digit];
                a.Opacity = values[0];
                b.Opacity = values[1];
                c.Opacity = values[2];
                d.Opacity = values[3];
                e.Opacity = values[4];
                f.Opacity = values[5];
                g.Opacity = values[6];
                h.Opacity = _digit > 10 ? 1 : 0;
                i.Opacity = _digit > 10 ? 1 : 0;
            }
        }
    }
}
