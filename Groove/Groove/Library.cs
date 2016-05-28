using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;

public class Item
{
    public string Link { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public string SmallImageUrl { get { return ImageUrl != null ? ImageUrl + "&w=64&h=64" : ImageUrl; } }
}

public class Tracks
{
    public List<Item> Items { get; set; }
}

public class Browse
{
    public Tracks Tracks { get; set; }
}

public class Library
{
    private const string clientId = "ClientID";
    private const string clientSecret = "ClientSecret";

    private const string service = "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13";
    private const string uri = "https://music.xboxlive.com//1/content/music/catalog/tracks/browse";
    private const string param = "?orderBy=MostPopular&maxitems=10&accessToken=Bearer+";
    private const string scope = "http://music.xboxlive.com";
    private const string grant = "client_credentials";
    private const string regex = ".*\"access_token\":\"(.*?)\".*";

    private HttpClient client = new HttpClient();
    private HttpResponseMessage response = new HttpResponseMessage();
    private Browse browse = new Browse();

    private async Task<string> token()
    {
        Dictionary<string, string> request = new Dictionary<string, string>()
        {
            {"client_id", clientId }, {"client_secret", clientSecret },
            { "scope", scope }, { "grant_type", grant }
        };
        response = await client.PostAsync(new Uri(service),
            new HttpFormUrlEncodedContent(request));
        return System.Net.WebUtility.UrlEncode(
            Regex.Match(await response.Content.ReadAsStringAsync(),
            regex, RegexOptions.IgnoreCase).Groups[1].Value);
    }

    public async void Init(ListView display)
    {
        response = await client.GetAsync(new Uri(uri + param + await token()));
        byte[] json = Encoding.Unicode.GetBytes(await response.Content.ReadAsStringAsync());
        using (MemoryStream stream = new MemoryStream(json))
        {
            browse = (Browse)new DataContractJsonSerializer(typeof(Browse)).ReadObject(stream);
        }
        display.ItemsSource = browse?.Tracks?.Items;
    }
}
