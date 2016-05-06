using System;
using System.Linq;
using System.Threading.Tasks;
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
    private const int size = 7;
    private bool won = false;
    private int[,] board = new int[size, size];
    private int player = 0;

    public Brush Background { get; set; }

    public void Show(string content, string title)
    {
        IAsyncOperation<IUICommand> command = new MessageDialog(content, title).ShowAsync();
    }

    public async Task<bool> Confirm(string content, string title, string ok, string cancel)
    {
        bool result = false;
        MessageDialog dialog = new MessageDialog(content, title);
        dialog.Commands.Add(new UICommand(ok, new UICommandInvokedHandler((cmd) => result = true)));
        dialog.Commands.Add(new UICommand(cancel, new UICommandInvokedHandler((cmd) => result = false)));
        await dialog.ShowAsync();
        return result;
    }

    public bool winner(int column, int row)
    {
        int total = 3; // Total Excluding Current
        int value = 0; // Value in Line
        int amend = 0; // Add or Remove
        // Check Vertical
        do
        {
            value++;
        }
        while (row + value < size &&
        board[column, row + value] == player);
        if (value > total)
        {
            return true;
        }
        value = 0;
        amend = 0;
        // Check Horizontal - From Left
        do
        {
            value++;
        }
        while (column - value >= 0 &&
        board[column - value, row] == player);
        if (value > total)
        {
            return true;
        }
        value -= 1; // Deduct Middle - Prevent double count
        // Then Right
        do
        {
            value++;
            amend++;
        }
        while (column + amend < size &&
        board[column + amend, row] == player);
        if (value > total)
        {
            return true;
        }
        value = 0;
        amend = 0;
        // Diagonal - Left Top
        do
        {
            value++;
        }
        while (column - value >= 0 && row - value >= 0 &&
        board[column - value, row - value] == player);
        if (value > total)
        {
            return true;
        }
        value -= 1; // Deduct Middle - Prevent double count
        // To Right Bottom
        do
        {
            value++;
            amend++;
        }
        while (column + amend < size && row + amend < size &&
        board[column + amend, row + amend] == player);
        if (value > total)
        {
            return true;
        }
        value = 0;
        amend = 0;
        // Diagonal - From Right Top
        do
        {
            value++;
        }
        while (column + value < size && row - value >= 0 &&
        board[column + value, row - value] == player);
        if (value > total)
        {
            return true;
        }
        value -= 1; // Deduct Middle - Prevent double count
        // To Left Bottom
        do
        {
            value++;
            amend++;
        }
        while (column - amend >= 0 &&
        row + amend < size &&
        board[column - amend, row + amend] == player);
        if (value > total)
        {
            return true;
        }
        return false;
    }

    private bool full()
    {
        for (int row = 0; row < size; row++)
        {
            for (int column = 0; column < size; column++)
            {
                if (board[column, row] == 0)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private Path getPiece(int player)
    {
        if ((player == 1)) // Draw X
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
            lines.Margin = new Thickness(4);
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
            circle.Margin = new Thickness(4);
            return circle;
        }
    }

    private void place(Grid grid, int column, int row)
    {
        for (int i = size - 1; i > -1; i--)
        {
            if (board[column, i] == 0)
            {
                board[column, i] = player;
                Canvas canvas = (Canvas)grid.Children.Single(
                    w => Grid.GetRow((Canvas)w) == i
                    && Grid.GetColumn((Canvas)w) == column);
                canvas.Children.Add(getPiece(player));
                row = i;
                break;
            }
        }
        if (winner(column, row))
        {
            won = true;
            Show("Player " + player + " has won!", "Four in Row");
        }
        else if (full())
        {
            Show("Board Full!", "Four in Row");
        }
        player = player == 1 ? 2 : 1; // Set Player
    }

    private void add(Grid grid, int row, int column)
    {
        Canvas canvas = new Canvas();
        canvas.Height = 40;
        canvas.Width = 40;
        canvas.Background = Background;
        canvas.Tapped += (object sender, TappedRoutedEventArgs e) =>
        {
            if (!won)
            {
                canvas = ((Canvas)(sender));
                row = (int)canvas.GetValue(Grid.RowProperty);
                column = (int)canvas.GetValue(Grid.ColumnProperty);
                if (board[column, 0] == 0) // Check Free Row
                {
                    place(grid, column, row);
                }
            }
            else
            {
                Show("Game Over!", "Four in Row");
            }
        };
        canvas.SetValue(Grid.ColumnProperty, column);
        canvas.SetValue(Grid.RowProperty, row);
        grid.Children.Add(canvas);
    }

    private void layout(ref Grid Grid)
    {
        player = 1;
        Grid.Children.Clear();
        Grid.ColumnDefinitions.Clear();
        Grid.RowDefinitions.Clear();
        // Setup Grid
        for (int Index = 0; (Index < size); Index++)
        {
            Grid.RowDefinitions.Add(new RowDefinition());
            Grid.ColumnDefinitions.Add(new ColumnDefinition());
        }
        // Setup Board
        for (int Column = 0; (Column < size); Column++)
        {
            for (int Row = 0; (Row < size); Row++)
            {
                add(Grid, Row, Column);
                board[Row, Column] = 0;
            }
        }
    }

    public async void New(Grid grid)
    {
        layout(ref grid);
        won = false;
        player = await Confirm("Who goes First?", "Four in Row", "X", "O") ? 1 : 2;
    }
}
