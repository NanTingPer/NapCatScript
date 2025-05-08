using Avalonia.Controls;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace NapCatScript.Desktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting { get; } = "Welcome to Avalonia!";
        public ObservableCollection<string> Items { get; } = [];
        private ViewModelBase currView;
        public ViewModelBase CurrView { get => currView; set => this.RaiseAndSetIfChanged(ref currView, value); }
        public MainWindowViewModel()
        {
            Items.Add("聊天");
            Items.Add("日志");
            Items.Add("网络");
            CurrView = new LogViewModel();
        }
    }
}
