using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Compression;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

public class Library
{
    private const string textExt = ".txt";
    private const string compressedExt = ".compressed";
    private CompressAlgorithm algorithm = CompressAlgorithm.Lzms;

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

    public async void New(TextBox display)
    {
        if (await Confirm("Create New?", "Compression App", "Yes", "No"))
        {
            display.Text = string.Empty;
        }
    }

    public async void Open(TextBox display)
    {
        try
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(textExt);
            picker.FileTypeFilter.Add(compressedExt);
            StorageFile file = await picker.PickSingleFileAsync();
            switch (file.FileType)
            {
                case textExt:
                    display.Text = await FileIO.ReadTextAsync(file);
                    break;
                case compressedExt:
                    using (MemoryStream stream = new MemoryStream())
                    using (IInputStream input = await file.OpenSequentialReadAsync())
                    using (Decompressor decompressor = new Decompressor(input))
                    using (IRandomAccessStream output = stream.AsRandomAccessStream())
                    {
                        long inputSize = input.AsStreamForRead().Length;
                        ulong outputSize = await RandomAccessStream.CopyAsync(decompressor, output);
                        output.Seek(0);
                        display.Text = await new StreamReader(output.AsStream()).ReadToEndAsync();
                        Show(string.Format("Decompressed {0} bytes to {1} bytes",
                            inputSize, outputSize), "Compression App");
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

    public async void Save(TextBox display)
    {
        try
        {
            StringBuilder text = new StringBuilder();
            FileSavePicker picker = new FileSavePicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeChoices.Add("Text File", new List<string>() { textExt });
            picker.FileTypeChoices.Add("Compressed File", new List<string>() { compressedExt });
            picker.DefaultFileExtension = textExt;
            StorageFile file = await picker.PickSaveFileAsync();
            switch (file.FileType)
            {
                case textExt:
                    await FileIO.WriteTextAsync(file, display.Text);
                    break;
                case compressedExt:
                    using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(display.Text)))
                    using (IRandomAccessStream input = stream.AsRandomAccessStream())
                    using (IRandomAccessStream output = await file.OpenAsync(FileAccessMode.ReadWrite))
                    using (Compressor compressor = new Compressor(output.GetOutputStreamAt(0), algorithm, 0))
                    {
                        ulong inputSize = await RandomAccessStream.CopyAsync(input, compressor);
                        bool finished = await compressor.FinishAsync();
                        ulong outputSize = output.Size;
                        Show(string.Format("Compressed {0} bytes to {1} bytes",
                            inputSize, outputSize), "Compression App");
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

    public void Sample(TextBox display)
    {
        StringBuilder text = new StringBuilder();
        for (int i = 0; i < 10; i++)
        {
            text.AppendLine("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris non massa diam. " +
            "Nunc luctus non lorem id imperdiet. Nunc quis mi nec enim malesuada commodo mollis eget nisl. " +
            "Sed vulputate in purus eu vulputate. Quisque commodo eu odio et malesuada. Duis porttitor, " +
            "lectus ut egestas placerat, purus nisi elementum diam, congue lacinia erat lectus sit amet felis. " +
            "Proin suscipit lobortis bibendum. Aliquam erat volutpat. Nunc vitae nulla nunc.\n");
        }
        display.Text = text.ToString();
    }
}
