using DigitalControl;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

public class Library
{
    private void layout(StackPanel panel)
    {
        for (int i = 0; i < 8; i++)
        {
            panel.Children.Add(new Digital()
            {
                Digit = 10,
                Height = 50,
                Margin = new Thickness(5)
            });
        }
        DispatcherTimer timer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromMilliseconds(250)
        };
        timer.Tick += (object sender, object e) =>
        {
            string time = DateTime.Now.ToString("HH:mm:ss");
            for (int i = 0; i < 8; i++)
            {
                string interval = time[i].ToString();
                ((Digital)panel.Children[i]).Digit =
                interval == ":" ? 11 : int.Parse(interval);
            }
        };
        timer.Start();
    }

    public void Init(StackPanel panel)
    {
        layout(panel);
    }
}
