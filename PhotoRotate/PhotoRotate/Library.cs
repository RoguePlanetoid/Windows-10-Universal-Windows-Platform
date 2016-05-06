using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

public class Library
{
    private static int angle;
    private static StorageFile file;
    private static WriteableBitmap bitmap;

    private static async Task<WriteableBitmap> read()
    {
        using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
        {
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(BitmapDecoder.JpegDecoderId, stream);
            uint width = decoder.PixelWidth;
            uint height = decoder.PixelHeight;
            if (angle % 180 != 0)
            {
                width = decoder.PixelHeight;
                height = decoder.PixelWidth;
            }
            Dictionary<int, BitmapRotation> angles = new Dictionary<int, BitmapRotation>()
            {
                { 0, BitmapRotation.None },
                { 90,  BitmapRotation.Clockwise90Degrees },
                { 180,  BitmapRotation.Clockwise180Degrees },
                { 270, BitmapRotation.Clockwise270Degrees },
                { 360, BitmapRotation.None }
            };
            BitmapTransform transform = new BitmapTransform();
            transform.Rotation = angles[angle];
            PixelDataProvider data = await decoder.GetPixelDataAsync(
            BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, transform,
            ExifOrientationMode.IgnoreExifOrientation, ColorManagementMode.DoNotColorManage);
            bitmap = new WriteableBitmap((int)width, (int)height);
            byte[] buffer = data.DetachPixelData();
            using (Stream pixels = bitmap.PixelBuffer.AsStream())
            {
                pixels.Write(buffer, 0, (int)pixels.Length);
            }
        }
        return bitmap;
    }

    private static async void write()
    {
        using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
        {
            BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
            encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore,
            (uint)bitmap.PixelWidth, (uint)bitmap.PixelHeight, 96.0, 96.0, bitmap.PixelBuffer.ToArray());
            await encoder.FlushAsync();
        }
    }

    public async void Open(Image display)
    {
        try
        {
            angle = 0;
            FileOpenPicker picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            StorageFile open = await picker.PickSingleFileAsync();
            if (open != null)
            {
                file = open;
                display.Source = await read();
            }
        }
        catch
        {
        }
    }

    public async void Save()
    {
        try
        {
            FileSavePicker picker = new FileSavePicker();
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeChoices.Add("Photo", new List<string>() { ".jpg" });
            picker.DefaultFileExtension = ".jpg";
            picker.SuggestedFileName = "Picture";
            StorageFile save = await picker.PickSaveFileAsync();
            if (save != null)
            {
                file = save;
                write();
            }
        }
        catch
        {
        }
    }

    public async void Rotate(Image display)
    {
        if (angle == 360) angle = 0;
        angle += 90;
        display.Source = await read();
    }
}
