using Avalonia.Media.Imaging;
using NapCatScript.Desktop.ViewModels;
using ReactiveUI;

namespace NapCatScript.Desktop.ChatViews.ChatTypeViews;

public class ChatImageViewModel : ViewModelBase
{
    private Bitmap _imagePath;

    public Bitmap ImagePath { get => _imagePath; set => this.RaiseAndSetIfChanged(ref _imagePath, value); }
}