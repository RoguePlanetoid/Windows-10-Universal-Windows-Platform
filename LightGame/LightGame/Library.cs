using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

public class Library
{
    private const int size = 7;
    private const int on = 1;
    private const int off = 0;
    private int moves = 0;
    private bool won = false;
    private int[,] board = new int[size, size];
    private Color lightOn = Colors.White;
    private Color lightOff = Colors.Black;

    public Brush Background { get; set; }

    public void Show(string content, string title)
    {
        IAsyncOperation<IUICommand> command = new MessageDialog(content, title).ShowAsync();
    }

    private bool winner()
    {
        for (int row = 0; row < size; row++)
        {
            for (int column = 0; column < size; column++)
            {
                if (board[column, row] == on)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void toggle(Grid grid, int row, int column)
    {
        board[row, column] = (board[row, column] == on ? off : on);
        Canvas canvas = (Canvas)grid.Children.Single(
                    w => Grid.GetRow((Canvas)w) == row
                    && Grid.GetColumn((Canvas)w) == column);
        canvas.Background = board[row, column] == on ?
            new SolidColorBrush(lightOn) : new SolidColorBrush(lightOff);
    }

    private void add(Grid grid, int row, int column)
    {
        Canvas canvas = new Canvas();
        canvas.Height = 40;
        canvas.Width = 40;
        canvas.Background = new SolidColorBrush(lightOn);
        canvas.Tapped += (object sender, TappedRoutedEventArgs e) =>
        {
            if (!won)
            {
                canvas = ((Canvas)(sender));
                row = (int)canvas.GetValue(Grid.RowProperty);
                column = (int)canvas.GetValue(Grid.ColumnProperty);
                toggle(grid, row, column);
                if (row > 0)
                {
                    toggle(grid, row - 1, column); // Toggle Left
                }
                if (row < (size - 1))
                {
                    toggle(grid, row + 1, column); // Toggle Right
                }
                if (column > 0)
                {
                    toggle(grid, row, column - 1); // Toggle Above
                }
                if (column < (size - 1))
                {
                    toggle(grid, row, column + 1); // Toggle Below
                }
                moves++;
                if (winner())
                {
                    Show("Well Done! You won in " + moves + " moves!", "Light Game");
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
        grid.Children.Clear();
        grid.ColumnDefinitions.Clear();
        grid.RowDefinitions.Clear();
        // Setup Grid
        grid.Background = Background;
        for (int index = 0; (index < size); index++)
        {
            grid.RowDefinitions.Add(new RowDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
        }
        for (int row = 0; (row < size); row++)
        {
            for (int column = 0; (column < size); column++)
            {
                add(grid, row, column);
            }
        }
    }

    public void New(Grid grid)
    {
        layout(ref grid);
        won = false;
        // Setup Board
        for (int column = 0; (column < size); column++)
        {
            for (int row = 0; (row < size); row++)
            {
                board[column, row] = on;
            }
        }
    }
}
