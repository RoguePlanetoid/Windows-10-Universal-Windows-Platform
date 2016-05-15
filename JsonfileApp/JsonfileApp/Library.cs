using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Json;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

public class Music
{
    public event PropertyChangedEventHandler PropertyChanged;
    private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    private string _album;
    private string _artist;
    private string _genre;

    public Music() { Id = Guid.NewGuid().ToString(); }

    public string Id { get; set; }
    public string Album { get { return _album; } set { _album = value; NotifyPropertyChanged(); } }
    public string Artist { get { return _artist; } set { _artist = value; NotifyPropertyChanged(); } }
    public string Genre { get { return _genre; } set { _genre = value; NotifyPropertyChanged(); } }
}

public class Library
{
    private const string filename = "file.json";
    private StorageFile file;
    private ObservableCollection<Music> collection = new ObservableCollection<Music>();

    private async void read()
    {
        try
        {
            file = await ApplicationData.Current.LocalFolder.GetFileAsync(filename);
            using (Stream stream = await file.OpenStreamForReadAsync())
            {
                collection = (ObservableCollection<Music>)
                    new DataContractJsonSerializer(typeof(ObservableCollection<Music>))
                    .ReadObject(stream);
            }
        }
        catch
        {
        }
    }

    private async void write()
    {
        try
        {
            file = await ApplicationData.Current.LocalFolder.CreateFileAsync(filename,
                CreationCollisionOption.ReplaceExisting);
            using (Stream stream = await file.OpenStreamForWriteAsync())
            {
                new DataContractJsonSerializer(typeof(ObservableCollection<Music>))
                    .WriteObject(stream, collection);
            }
        }
        catch
        {
        }
    }

    public ObservableCollection<Music> Collection { get { return collection; } }

    public Library()
    {
        read();
    }

    public void Add(FlipView display)
    {

        collection.Insert(0, new Music());
        display.SelectedIndex = 0;
    }

    public void Save()
    {
        write();
    }

    public void Remove(FlipView display)
    {
        if (display.SelectedItem != null)
        {
            collection.Remove(collection.Where(w => w.Id ==
            ((Music)display.SelectedValue).Id).Single());
            write();
        }
    }

    public async void Delete(FlipView display)
    {
        try
        {
            collection = new ObservableCollection<Music>();
            display.ItemsSource = Collection;
            file = await ApplicationData.Current.LocalFolder.GetFileAsync(filename);
            await file.DeleteAsync();
        }
        catch
        {

        }
    }
}
