using System;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Net;
using System.Reactive;
using System.Threading.Tasks;
using NapCatScript.Core.JsonFormat;
using static NapCatScript.Core.MsgHandle.Utils;

namespace NapCatScript.Desktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private bool _webUiInit = false;
        private ViewModelBase currView;
        private string _selectedItem;
        
        public IReactiveCommand ListBoxPropertyChangedCommand {get; private set; }
        private ViewModelBase? _logViewModel;
        public ViewModelBase LogViewModel
        {
            get {
                if (_logViewModel is null) {
                    _logViewModel = new LogViewModel();
                }
                return _logViewModel;
            }
        }
        
        private ViewModelBase? _netWorkModel;
        public ViewModelBase NetWorkModel
        {
            get {
                if (_netWorkModel is null) {
                    _netWorkModel = new NetWorkViewModel();
                }
                return _netWorkModel;
            }
        }

        private ViewModelBase? _webUIConnectionViewModel;
        public ViewModelBase WebUIConnectionViewModel
        {
            get {
                if (_webUIConnectionViewModel is null) {
                    _webUIConnectionViewModel = new WebUIConnectionViewModel();
                }
                return _webUIConnectionViewModel;
            }
        }
        public ObservableCollection<string> Items { get; } = [];
        public string SelectedItem { get => _selectedItem; set => this.RaiseAndSetIfChanged(ref _selectedItem, value); }
        public ViewModelBase CurrView { get => currView; set => this.RaiseAndSetIfChanged(ref currView, value); }
        public MainWindowViewModel()
        {
            this.WhenAnyValue(f => f.SelectedItem)
                .WhereNotNull()
                .Subscribe(ListBoxPropertyChanged);

            Interactions.WebUIConnectionEvent.RegisterHandler(WebUILoginEvent);
            Items.Add("连接");
            Items.Add("聊天");
            Items.Add("日志");
            Items.Add("网络");
            CurrView = WebUIConnectionViewModel;
        }

        private void ListBoxPropertyChanged(string str)
        {/*
            if (CurrView is LogViewModel log){
                log.Dispose();
            }
            CurrView = null;
            GC.Collect();*/
            
            if(_webUiInit == false)
                return;
            if(str == "网络") CurrView = NetWorkModel;
            if(str == "日志") CurrView = LogViewModel;
            if(str == "连接") CurrView = WebUIConnectionViewModel;
        }

        private async Task WebUILoginEvent(IInteractionContext<(string,string),Unit> interaction)
        {
            (string webURL, string token) connectionValue = interaction.Input;
            interaction.SetOutput(Unit.Default);
            if(connectionValue.token == ConfigValue.WebToken && connectionValue.webURL == ConfigValue.WebUri)
                return;
            
            var rms = await WebUILogin(connectionValue.webURL, connectionValue.token);
            var requJson = await rms.Content.ReadAsStringAsync();
            if(!requJson.GetJsonElement(out var json))
                return;
            if(!json.TryGetPropertyValue("message", out var Jvalue))
                return;

            if(Jvalue.GetString() != "success")
                return;
            
            var aut = await GetAuthentication(connectionValue.webURL, connectionValue.token);
            ConfigValue.WebToken = connectionValue.token;
            ConfigValue.AuthToken = aut;
            ConfigValue.WebUri = connectionValue.webURL;
            CurrView = LogViewModel;
            _webUiInit = true;
        }
    }
}
