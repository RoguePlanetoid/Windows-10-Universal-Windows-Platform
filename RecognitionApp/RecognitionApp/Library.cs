using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Globalization;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

public class Library
{
    private const string textExt = ".txt";
    private const string imageExt = ".jpg";

    public async Task<bool> Confirm(string content, string title, string ok, string cancel)
    {
        bool result = false;
        MessageDialog dialog = new MessageDialog(content, title);
        dialog.Commands.Add(new UICommand(ok, new UICommandInvokedHandler((cmd) => result = true)));
        dialog.Commands.Add(new UICommand(cancel, new UICommandInvokedHandler((cmd) => result = false)));
        await dialog.ShowAsync();
        return result;
    }

    public async void New(TextBox display)
    {
        if (await Confirm("Create New?", "Recognition App", "Yes", "No"))
        {
            display.Text = string.Empty;
        }
    }

    public async void Open(Image source, TextBox target)
    {
        try
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(textExt);
            picker.FileTypeFilter.Add(imageExt);
            StorageFile file = await picker.PickSingleFileAsync();
            switch (file.FileType)
            {
                case textExt:
                    target.Text = await FileIO.ReadTextAsync(file);
                    break;
                case imageExt:
                    using (IRandomAccessStream stream = await file.OpenReadAsync())
                    {
                        BitmapDecoder bitmapDecoder = await BitmapDecoder.CreateAsync(stream);
                        SoftwareBitmap softwareBitmap = await bitmapDecoder.GetSoftwareBitmapAsync(
                            BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                        OcrEngine engine = OcrEngine.TryCreateFromLanguage(new Language("en-us"));
                        OcrResult ocrResult = await engine.RecognizeAsync(softwareBitmap);
                        target.Text = ocrResult.Text;
                        stream.Seek(0);
                        BitmapImage image = new BitmapImage();
                        image.SetSource(stream);
                        source.Source = image;
                    }
                    break;
                default:
                    break;
            }
        }
        catch
        {

        }
    }

    public async void Save(Image source, TextBox target)
    {
        try
        {
            FileSavePicker picker = new FileSavePicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeChoices.Add("Image File", new List<string>() { imageExt });
            picker.FileTypeChoices.Add("Text File", new List<string>() { textExt });
            picker.DefaultFileExtension = textExt;
            StorageFile file = await picker.PickSaveFileAsync();
            switch (file.FileType)
            {
                case textExt:
                    await FileIO.WriteTextAsync(file, target.Text);
                    break;
                case imageExt:
                    using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
                        RenderTargetBitmap render = new RenderTargetBitmap();
                        await render.RenderAsync(target);
                        IBuffer buffer = await render.GetPixelsAsync();
                        encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore,
                        (uint)render.PixelWidth, (uint)render.PixelHeight, 96.0, 96.0, buffer.ToArray());
                        await encoder.FlushAsync();
                        target = null;
                        buffer = null;
                        encoder = null;
                    }
                    break;
                default:
                    break;
            }
        }
        catch
        {

        }
    }

    public void Sample(TextBox target)
    {
        target.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris non massa diam. " +
            "Nunc luctus non lorem id imperdiet. Nunc quis mi nec enim malesuada commodo mollis eget nisl. " +
            "Sed vulputate in purus eu vulputate. Quisque commodo eu odio et malesuada. Duis porttitor, " +
            "lectus ut egestas placerat, purus nisi elementum diam, congue lacinia erat lectus sit amet felis. " +
            "Proin suscipit lobortis bibendum. Aliquam erat volutpat. Nunc vitae nulla nunc.";
    }
}
