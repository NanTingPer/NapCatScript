using System;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace NapCatScript.Desktop.ChatViews.ChatTypeViews;

public static class ImageHelper
{
    public static Bitmap LoadImage(string imagePath)
    {
        return new Bitmap(AssetLoader.Open(new Uri(imagePath)));
    }

    public static Bitmap LoadImage(Uri imageUri)
    {
        return new Bitmap(AssetLoader.Open(imageUri));
    }
}