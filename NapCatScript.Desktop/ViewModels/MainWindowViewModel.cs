using System;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace NapCatScript.Desktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase currView;
        private string _selectedItem;
        
        public IReactiveCommand ListBoxPropertyChangedCommand {get; private set; }
        public LogViewModel LogViewModel {get; init; }
        public ObservableCollection<string> Items { get; } = [];
        public string SelectedItem { get => _selectedItem; set => this.RaiseAndSetIfChanged(ref _selectedItem, value); }
        public ViewModelBase CurrView { get => currView; set => this.RaiseAndSetIfChanged(ref currView, value); }
        public MainWindowViewModel()
        {
            this.WhenAnyValue(f => f.SelectedItem)
                .WhereNotNull()
                .Subscribe(ListBoxPropertyChanged);
            Items.Add("聊天");
            Items.Add("日志");
            Items.Add("网络");
            LogViewModel = new LogViewModel();
            CurrView = LogViewModel;
        }

        private void ListBoxPropertyChanged(string str)
        {
            if (CurrView is LogViewModel log){
                log.Dispose();
            }
            CurrView = null;
            GC.Collect();
            if(str == "网络")
                CurrView = new NetWorkViewModel();
            if(str == "日志")
                CurrView = LogViewModel;
        }
    }
}
