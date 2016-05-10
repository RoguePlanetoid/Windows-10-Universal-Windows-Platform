using System;
using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

public class Library
{
    private const int size = 3;
    private int spins = 0;
    private int[] board = new int[size];
    private string[] values = { "Apple", "Lemon", "Orange", "Strawberry", "Blackberry", "Cherry" };
    private Random random = new Random((int)DateTime.Now.Ticks);

    public Brush Background { get; set; }

    public void Show(string content, string title)
    {
        IAsyncOperation<IUICommand> command = new MessageDialog(content, title).ShowAsync();
    }

    private Path ellipse(int x, int y, Color fill)
    {
        return new Path()
        {
            Data = new EllipseGeometry()
            {
                Center = new Point(20, 20),
                RadiusX = x,
                RadiusY = y,
            },
            Stroke = new SolidColorBrush(fill),
            Fill = new SolidColorBrush(fill),
            StrokeThickness = 5,
            Margin = new Thickness(10)
        };
    }

    private UIElement fruit(int type)
    {
        switch (type)
        {
            case 0: // Apple
                return ellipse(20, 18, Colors.LawnGreen);
            case 1: // Lemon
                return ellipse(16, 20, Colors.Yellow);
            case 2: // Orange
                return ellipse(20, 20, Colors.Orange);
            case 3: // Strawberry
                return new Polygon()
                {
                    Points = new PointCollection
                    {
                        new Point(0, 0), new Point(15, 30), new Point(30, 0)
                    },
                    Stretch = Stretch.Uniform,
                    StrokeLineJoin = PenLineJoin.Round,
                    Height = 40,
                    Width = 40,
                    Fill = new SolidColorBrush(Colors.IndianRed),
                    Stroke = new SolidColorBrush(Colors.IndianRed),
                    StrokeThickness = 5,
                    Margin = new Thickness(10)
                };
            case 4: // Blackberry
                return new Path()
                {
                    Data = new GeometryGroup()
                    {
                        Children = new GeometryCollection()
                        {
                            new EllipseGeometry()
                            {
                                Center = new Point(10, 10),
                                RadiusX = 8, RadiusY = 8
                            },
                            new EllipseGeometry()
                            {
                                Center = new Point(30, 10),
                                RadiusX = 8, RadiusY = 8
                            },
                            new EllipseGeometry()
                            {
                                Center = new Point(20, 30),
                                RadiusX = 8, RadiusY = 8
                            }
                        }
                    },
                    Fill = new SolidColorBrush(Colors.MediumPurple),
                    StrokeThickness = 5,
                    Margin = new Thickness(10)
                };
            case 5: // Cherry
                return new Path()
                {
                    Data = new GeometryGroup()
                    {
                        Children = new GeometryCollection()
                        {
                            new EllipseGeometry()
                            {
                                Center = new Point(10, 25),
                                RadiusX = 8, RadiusY = 8
                            },
                            new EllipseGeometry()
                            {
                                Center = new Point(30, 25),
                                RadiusX = 8, RadiusY = 8
                            }
                        }
                    },
                    Fill = new SolidColorBrush(Colors.MediumVioletRed),
                    StrokeThickness = 5,
                    Margin = new Thickness(10)
                };
            default:
                return null;
        }
    }

    private void add(ref Grid grid, int column, int type)
    {
        Viewbox viewbox = new Viewbox()
        {
            Height = 100,
            Width = 100,
            Stretch = Stretch.UniformToFill,
            Child = fruit(type)
        };
        viewbox.SetValue(Grid.ColumnProperty, column);
        grid.Children.Add(viewbox);
    }

    private void layout(Grid grid)
    {
        grid.Children.Clear();
        grid.ColumnDefinitions.Clear();
        // Setup Grid
        for (int column = 0; (column < size); column++)
        {
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            board[column] = random.Next(0, 6);
            add(ref grid, column, board[column]);
        }
    }

    public void New(Grid grid)
    {
        layout(grid);
        spins++;
        // Check Winner        
        if (board.All(a => a == board.First()))
        {
            Show(("Spin " + spins + " matched " + values[board.First()]), "Fruit Game");
            spins = 0;
        }
    }
}
