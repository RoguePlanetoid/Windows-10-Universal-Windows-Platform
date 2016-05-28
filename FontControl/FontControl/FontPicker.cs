using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace FontControl
{
    public class FontPicker : ComboBox
    {
        private string[] fonts =
        {
            "Arial", "Calibri", "Cambria", "Cambria Math", "Comic Sans MS", "Courier New",
            "Ebrima", "Gadugi", "Georgia", "Javanese Text", "Leelawadee UI", "Lucida Console",
            "Malgun Gothic", "Microsoft Himalaya", "Microsoft JhengHei", "Microsoft JhengHei UI",
            "Microsoft New Tai Lue", "Microsoft PhagsPa", "Microsoft Tai Le", "Microsoft YaHei",
            "Microsoft YaHei UI", "Microsoft Yi Baiti", "Mongolian Baiti", "MV Boli", "Myanmar Text",
            "Nirmala UI", "Segoe Print", "Segoe UI", "Segoe UI Emoji", "Segoe UI Historic",
            "Segoe UI Symbol", "SimSun", "NSimSun", "Times New Roman",  "Trebuchet MS",
            "Verdana", "Webdings", "Wingdings", "Yu Gothic", "Yu Gothic UI"
        };

        public FontPicker()
        {
            this.ItemTemplate = (DataTemplate)XamlReader.Load(
            "<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">" +
            "<TextBlock Text=\"{Binding}\" FontFamily=\"{Binding}\"/>" +
            "</DataTemplate>");
            this.ItemsSource = fonts;
        }

        public FontFamily Selected
        {
            get { return new FontFamily((string)SelectedItem); }
        }
    }
}
