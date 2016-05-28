using System;
using System.Text;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;

public class Library
{
    private Random random = new Random((int)DateTime.Now.Ticks);

    public void Schedule(DateTime when)
    {
        StringBuilder template = new StringBuilder();
        template.Append("<toast><visual version='2'><binding template='ToastText02'>");
        template.AppendFormat("<text id='2'>Enter Message:</text>");
        template.Append("</binding></visual><actions>");
        template.Append("<input id='message' type='text'/>");
        template.Append("<action activationType='foreground' content='ok' arguments='ok'/>");
        template.Append("</actions></toast>");
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(template.ToString());
        ScheduledToastNotification toast = new ScheduledToastNotification(xml, when);
        toast.Id = random.Next(1, 100000000).ToString();
        ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast);
    }
}
