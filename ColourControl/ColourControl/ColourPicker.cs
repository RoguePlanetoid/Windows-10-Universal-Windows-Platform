using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace ColourControl
{
    public class ColourPicker : ComboBox
    {
        private static IEnumerable<string> splitCapital(string text)
        {
            Regex regex = new Regex(@"\p{Lu}\p{Ll}*");
            foreach (Match match in regex.Matches(text))
            {
                yield return match.Value;
            }
        }

        private Dictionary<string, Color> colours = typeof(Colors)
        .GetRuntimeProperties()
        .Select(c => new
        {
            Color = (Color)c.GetValue(null),
            Name = string.Join(" ", splitCapital(c.Name))
        }).ToDictionary(x => x.Name, x => x.Color);

        public ColourPicker()
        {
            this.ItemTemplate = (DataTemplate)XamlReader.Load(
            "<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">" +
            "<StackPanel Orientation=\"Horizontal\">" +
            "<Rectangle Width=\"20\" Height=\"15\" Margin=\"5,0\"><Rectangle.Fill>" +
            "<SolidColorBrush Color=\"{Binding Value}\"/></Rectangle.Fill></Rectangle>" +
            "<TextBlock VerticalAlignment=\"Center\" Text=\"{Binding Key}\"/>" +
            "</StackPanel></DataTemplate>");
            this.SelectedValuePath = "Value";
            this.ItemsSource = colours;
        }

        public Color Selected
        {
            get { return ((KeyValuePair<string, Color>)SelectedItem).Value; }
        }
    }
}
