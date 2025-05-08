using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static NapCatScript.Core.JsonFormat.Utils;
using NapCatScript.Core.MsgHandle;
using ReactiveUI;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NapCatScript.Desktop.ViewModels;

public class LogViewModel : ViewModelBase
{
    private ObservableCollection<string> _log = new ObservableCollection<string>();
    private string _logString = "";
    public ObservableCollection<string> Log { get => _log; private set => this.RaiseAndSetIfChanged(ref _log, value); }
    public string LogString { get => _logString; private set => this.RaiseAndSetIfChanged(ref _logString, value); }
    
    public LogViewModel()
    {
        _ = Task.Run(GetLog);
    }

    public async void GetLog()
    {
        await foreach (string? logc in Utils.GetLoging("http://127.0.0.1:6099", "6099", "napcat")){
            if (logc == null)
                continue;
            if (logc is string st && !string.IsNullOrEmpty(st)) {
                try {
                    if (logc.Trim().Substring("data:".Length).GetJsonElement(out var json)) {
                        if (json.TryGetPropertyValue("level", out var propValue)) {
                            if (propValue.GetString() != "info") {
                                continue;
                            }
                        }

                        if (json.TryGetPropertyValue("message", out var msg)) {
                            Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                            {
                                LogString += msg.GetString() + "\r\n";
                                while (LogString.Length > 1000) {
                                    LogString = LogString.Substring(1);
                                }
                                //Log.Insert(0, logc);
                            });
                        }
                    }
                }
                catch (Exception e) {

                }
            }
        }
    }
}
