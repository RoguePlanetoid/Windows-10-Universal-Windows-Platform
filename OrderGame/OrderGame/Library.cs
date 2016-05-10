using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

public class Library
{
    private const int size = 6;
    private const int total = size * size;
    private DateTime timer;
    private ObservableCollection<int> items = new ObservableCollection<int>();
    private Random random = new Random((int)DateTime.Now.Ticks);

    public void Show(string content, string title)
    {
        IAsyncOperation<IUICommand> command = new MessageDialog(content, title).ShowAsync();
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

    private bool winner()
    {
        return items.OrderBy(o => o).ToList().SequenceEqual(items.ToList());
    }

    private void layout(ref GridView grid)
    {
        timer = DateTime.UtcNow;
        grid.IsEnabled = true;
        grid.ItemsSource = null;
        items = new ObservableCollection<int>();
        List<int> numbers = select(1, total, total);
        int index = 0;
        while (index < numbers.Count)
        {
            items.Add(numbers[index]);
            index++;
        }
        grid.ItemsSource = items;
    }

    public void New(GridView grid)
    {
        layout(ref grid);
    }

    public void Order(GridView grid)
    {
        if (winner())
        {
            TimeSpan duration = (DateTime.UtcNow - timer).Duration();
            Show(string.Format("Well Done! Completed in {0} Hours, {1} Minutes and {2} Seconds!",
                duration.Hours, duration.Minutes, duration.Seconds), "Order Game");
            grid.IsEnabled = false;
        }
    }
}
