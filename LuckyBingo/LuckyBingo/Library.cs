using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

public class Library
{
    private int index = 0;
    private List<int> numbers = null;
    private ObservableCollection<Grid> items = new ObservableCollection<Grid>();
    private Random random = new Random((int)DateTime.Now.Ticks);

    private List<int> select(int start, int finish, int total)
    {
        int number;
        List<int> numbers = new List<int>();
        while ((numbers.Count < total)) // Select Numbers
        {
            // Random Number between Start and Finish
            number = random.Next(start, finish + 1);
            if ((!numbers.Contains(number)) || (numbers.Count < 1))
            {
                numbers.Add(number); // Add if number Chosen or None
            }
        }
        return numbers;
    }

    private void pick(ref GridView grid)
    {
        if (index < numbers.Count)
        {
            int number = numbers[index];
            TextBlock text = new TextBlock()
            {
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 24,
                Text = number.ToString(),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid container = new Grid()
            {
                Margin = new Thickness(2),
                Width = 48,
                Height = 48
            };
            Ellipse ball = new Ellipse();
            Color fill;
            ball.Width = container.Width;
            ball.Height = container.Height;
            if (number >= 1 && number <= 10)
            {
                fill = Colors.Magenta;
            }
            else if (number >= 11 && number <= 20)
            {
                fill = Colors.Blue;
            }
            else if (number >= 21 && number <= 30)
            {
                fill = Colors.DarkGreen;
            }
            else if (number >= 31 && number <= 40)
            {
                fill = Colors.YellowGreen;
            }
            else if (number >= 41 && number <= 50)
            {
                fill = Colors.Orange;
            }
            else if (number >= 51 && number <= 60)
            {
                fill = Colors.DarkBlue;
            }
            else if (number >= 61 && number <= 70)
            {
                fill = Colors.Red;
            }
            else if (number >= 71 && number <= 80)
            {
                fill = Colors.DarkCyan;
            }
            else if (number >= 81 && number <= 90)
            {
                fill = Colors.Purple;
            }
            ball.Fill = new SolidColorBrush(fill);
            container.Children.Add(ball);
            container.Children.Add(text);
            items.Add(container);
            index++;
            grid.ItemsSource = items;
        }
    }

    private void layout(ref GridView grid)
    {
        grid.ItemsSource = null;
        items = new ObservableCollection<Grid>();
        index = 0;
        numbers = select(1, 90, 90);
    }

    public void New(GridView grid)
    {
        layout(ref grid);
    }

    public void Pick(GridView grid)
    {
        if (numbers == null) layout(ref grid);
        pick(ref grid);
    }
}
