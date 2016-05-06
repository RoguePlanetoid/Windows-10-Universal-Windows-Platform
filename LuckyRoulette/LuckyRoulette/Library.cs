using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

public class Library
{
    private int spins = 0;
    private int spinValue = 0;
    private int pickValue = 0;
    private Grid pocket;
    private Random random = new Random((int)DateTime.Now.Ticks);

    public void Show(string content, string title)
    {
        IAsyncOperation<IUICommand> command = new MessageDialog(content, title).ShowAsync();
    }

    private bool isOdd(int value)
    {
        return (value % 2 != 0);
    }

    private void pick()
    {
        spins++;
        pocket.Children.Clear();
        spinValue = random.Next(0, 36); // Random 0 - 36
        Grid container = new Grid()
        {
            Height = 220,
            Width = 220
        };
        TextBlock text = new TextBlock()
        {
            Foreground = new SolidColorBrush(Colors.White),
            FontSize = 120,
            Text = spinValue.ToString(),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        Color fill;
        if (spinValue >= 1 && spinValue <= 10 || spinValue >= 19 && spinValue <= 28)
        {
            fill = isOdd(spinValue) ? Colors.Black : Colors.DarkRed;
        }
        else if (spinValue >= 11 && spinValue <= 18 || spinValue >= 29 && spinValue <= 36)
        {
            fill = isOdd(spinValue) ? Colors.DarkRed : Colors.Black;
        }
        else if (spinValue == 0)
        {
            fill = Colors.DarkGreen;
        }
        container.Background = new SolidColorBrush(fill);
        container.Children.Add(text);
        pocket.Children.Add(container);
        if (spinValue == pickValue) // Check Win
        {
            Show(("Spin " + spins + " matched " + spinValue), "Lucky Roulette");
            spins = 0;
        }
    }

    private void layout(ref Grid grid)
    {
        grid.Children.Clear();
        spins = 0;
        List<int> values = Enumerable.Range(0, 36).ToList();
        StackPanel panel = new StackPanel();
        pocket = new Grid()
        {
            Height = 220,
            Width = 220,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        ComboBox combobox = new ComboBox()
        {
            Margin = new Thickness(10),
            HorizontalAlignment = HorizontalAlignment.Center
        };
        combobox.ItemsSource = values;
        combobox.SelectedIndex = 0;
        combobox.SelectionChanged += (object sender, SelectionChangedEventArgs e) =>
        {
            pickValue = (int)((ComboBox)combobox).SelectedValue;
        };
        panel.Children.Add(pocket);
        panel.Children.Add(combobox);
        grid.Children.Add(panel);
    }

    public void New(Grid grid)
    {
        layout(ref grid);
    }

    public void Pick(Grid grid)
    {
        if (pocket == null) layout(ref grid);
        pick();
    }
}
