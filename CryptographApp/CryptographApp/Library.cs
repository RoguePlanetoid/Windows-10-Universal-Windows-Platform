using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;

public class Library
{
    private static byte[] values =
    {
        1, 4, 8, 15, 16, 23, 42, 108, 1, 4, 8, 15, 16, 23, 42, 108,
        1, 4, 8, 15, 16, 23, 42, 108, 1, 4, 8, 15, 16, 23, 42, 108
    };
    private static SymmetricKeyAlgorithmProvider algorithm =
        SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);
    private static CryptographicKey key =
        algorithm.CreateSymmetricKey(CryptographicBuffer.CreateFromByteArray(values));
    private static IBuffer iv = values.AsBuffer();

    public async void Open(TextBox display)
    {
        try
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".txt");
            StorageFile file = await picker.PickSingleFileAsync();
            display.Text = await FileIO.ReadTextAsync(file);
        }
        catch
        {

        }
    }

    public async void Save(TextBox display)
    {
        try
        {
            FileSavePicker picker = new FileSavePicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            picker.DefaultFileExtension = ".txt";
            picker.SuggestedFileName = "Document";
            StorageFile file = await picker.PickSaveFileAsync();
            if (file != null)
            {
                await FileIO.WriteTextAsync(file, display.Text);
            }
        }
        catch
        {

        }
    }

    public void Encrypt(TextBox display)
    {
        try
        {
            IBuffer buffer = CryptographicBuffer.ConvertStringToBinary(display.Text,
            BinaryStringEncoding.Utf8);
            IBuffer bufferEncrypt = CryptographicEngine.Encrypt(key, buffer, iv);
            display.Text = CryptographicBuffer.EncodeToBase64String(bufferEncrypt);
        }
        catch
        {

        }
    }

    public void Decrypt(TextBox display)
    {
        try
        {
            IBuffer buffer = Convert.FromBase64String(display.Text).AsBuffer();
            IBuffer bufferDecrypt = CryptographicEngine.Decrypt(key, buffer, iv);
            display.Text = CryptographicBuffer.ConvertBinaryToString(
                BinaryStringEncoding.Utf8, bufferDecrypt);
        }
        catch
        {

        }
    }
}
