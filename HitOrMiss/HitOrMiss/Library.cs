using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

public class Library
{
    private const int score = 18;
    private const int size = 6;
    private const int hit = 1;
    private const int miss = 0;
    private int moves = 0;
    private int hits = 0;
    private int misses = 0;
    private bool won = false;
    private List<string> values = new List<string> { "Miss", "Hit" };
    private int[,] board = new int[size, size];
    private Random random = new Random((int)DateTime.Now.Ticks);

    public Brush Background { get; set; }

    public void Show(string content, string title)
    {
        IAsyncOperation<IUICommand> command = new MessageDialog(content, title).ShowAsync();
    }

    private Path getPiece(string value)
    {
        if ((value == "Miss")) // Draw X
        {
            Path lines = new Path();
            LineGeometry line1 = new LineGeometry();
            LineGeometry line2 = new LineGeometry();
            GeometryGroup linegroup = new GeometryGroup();
            line1.StartPoint = new Point(0, 0);
            line1.EndPoint = new Point(30, 30);
            line2.StartPoint = new Point(30, 0);
            line2.EndPoint = new Point(0, 30);
            linegroup.Children.Add(line1);
            linegroup.Children.Add(line2);
            lines.Data = linegroup;
            lines.Stroke = new SolidColorBrush(Colors.Red);
            lines.StrokeThickness = 2;
            lines.Margin = new Thickness(8);
            return lines;
        }
        else // Draw O
        {
            EllipseGeometry ellipse = new EllipseGeometry();
            Path circle = new Path();
            ellipse.Center = new Point(15, 15);
            ellipse.RadiusX = 15;
            ellipse.RadiusY = 15;
            circle.Data = ellipse;
            circle.Stroke = new SolidColorBrush(Colors.Blue);
            circle.StrokeThickness = 2;
            circle.Margin = new Thickness(8);
            return circle;
        }
    }

    private void add(ref Grid grid, int row, int column)
    {
        Canvas canvas = new Canvas();
        canvas.Height = 46;
        canvas.Width = 46;
        canvas.Background = Background;
        canvas.Tapped += (object sender, TappedRoutedEventArgs e) =>
        {
            if (!won)
            {
                int selected;
                canvas = ((Canvas)(sender));
                row = (int)canvas.GetValue(Grid.RowProperty);
                column = (int)canvas.GetValue(Grid.ColumnProperty);
                selected = board[row, column];
                if (canvas.Children.Count <= 0)
                {
                    canvas.Children.Clear();
                    canvas.Children.Add(getPiece(values[selected]));
                    if (selected == hit)
                    {
                        hits++;
                    }
                    else if (selected == miss)
                    {
                        misses++;
                    }
                    moves++;
                }
                if (moves < (size * size) && misses < score)
                {
                    if (hits == score)
                    {
                        Show("Well Done! You scored " + hits + " hits and " + misses + " misses!", "Hit or Miss");
                        won = true;
                    }
                }
                else
                {
                    Show("Game Over! You scored " + hits + " hits and " + misses + " misses!", "Hit or Miss");
                    won = true;
                }
            }
        };
        canvas.SetValue(Grid.ColumnProperty, column);
        canvas.SetValue(Grid.RowProperty, row);
        grid.Children.Add(canvas);
    }

    private void layout(ref Grid grid)
    {
        moves = 0;
        hits = 0;
        misses = 0;
        grid.Children.Clear();
        grid.ColumnDefinitions.Clear();
        grid.RowDefinitions.Clear();
        // Setup Grid
        for (int index = 0; (index < size); index++)
        {
            grid.RowDefinitions.Add(new RowDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
        }
        for (int row = 0; (row < size); row++)
        {
            for (int column = 0; (column < size); column++)
            {
                add(ref grid, row, column);
            }
        }
    }

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

    public void New(Grid grid)
    {
        layout(ref grid);
        List<int> values = new List<int>();
        List<int> indices = new List<int>();
        won = false;
        int counter = 0;
        while (values.Count < (size * size))
        {
            values.Add(hit);
            values.Add(miss);
        }
        indices = select(1, (size * size), (size * size));
        // Setup Board
        for (int column = 0; (column < size); column++)
        {
            for (int row = 0; (row < size); row++)
            {
                board[column, row] = values[indices[counter] - 1];
                counter++;
            }
        }
    }
}
