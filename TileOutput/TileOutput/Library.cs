using System;
using System.Linq;
using System.Text;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Xaml.Controls;

public class Item
{
    public string Id { get; set; }
    public string Content { get; set; }
    public string Time { get; set; }
}

public class Library
{
    private Random random = new Random((int)DateTime.Now.Ticks);

    public void Add(ref ListBox display, string value, TimeSpan occurs)
    {
        DateTime when = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
            occurs.Hours, occurs.Minutes, occurs.Seconds);
        if (when > DateTime.Now)
        {
            StringBuilder template = new StringBuilder();
            template.Append("<tile><visual version=\"4\">");
            template.Append("<binding template=\"TileSquare150x150Text03\">");
            template.AppendFormat("<text id=\"1\">{0}</text>", value);
            template.AppendFormat("<text id=\"2\">{0}</text>", when.ToLocalTime().ToString("HH:mm"));
            template.Append("</binding></visual></tile>");
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(template.ToString());

            ScheduledTileNotification tile = new ScheduledTileNotification(xml, when);
            tile.Id = random.Next(1, 100000000).ToString();
            TileUpdateManager.CreateTileUpdaterForApplication().AddToSchedule(tile);
            display.Items.Add(new Item { Id = tile.Id, Content = value, Time = when.ToString() });
        }
    }

    public void Remove(ListBox display)
    {
        if (display.SelectedIndex > -1)
        {
            TileUpdateManager.CreateTileUpdaterForApplication().GetScheduledTileNotifications().Where(
                p => p.Id.Equals(((Item)display.SelectedItem).Id)).SingleOrDefault();
            display.Items.RemoveAt(display.SelectedIndex);
        }
    }
}
