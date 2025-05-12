using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using static NapCatScript.Core.JsonFormat.Utils;
using ReactiveUI;
using System.Threading.Tasks;
using NapCatScript.Core;
using NapCatScript.Core.Services;
using Utils = NapCatScript.Core.MsgHandle.Utils;

namespace NapCatScript.Desktop.ViewModels;

public class LogViewModel : ViewModelBase, IDisposable
{
    private ObservableCollection<string> _log = new ObservableCollection<string>();
    private string _logString = "";
    public ObservableCollection<string> Log { get => _log; private set => this.RaiseAndSetIfChanged(ref _log, value); }
    public string LogString { get => _logString; private set => this.RaiseAndSetIfChanged(ref _logString, value); }
    private CancellationTokenSource CancellationTokenSource { get;} = new CancellationTokenSource();
    private CancellationToken CToken {get => CancellationTokenSource.Token;}
    public LogViewModel()
    {
        _ = Task.Run(GetLog, CToken);
    }

    ~LogViewModel()
    {
        Debug.WriteLine("解构");
    }

    public void Dispose()
    {
        CancellationTokenSource.Cancel();
    }

    public async void GetLog()
    {
        await foreach (string? logc in Utils.GetLoging(CoreConfigValueAndObject.HttpUri, "6099", "napcat")) {
            if (logc == null)
                continue;
            if (string.IsNullOrEmpty(logc))
                continue;
            try {
                if (logc.Trim().Substring("data:".Length).GetJsonElement(out var json)) {
                    if (json.TryGetPropertyValue("level", out var propValue)) {
                        if (propValue.GetString() != "info") {
                            continue;
                        }
                    }

                    if (json.TryGetPropertyValue("message", out var msg)) {
                        Avalonia.Threading.Dispatcher.UIThread.Post(() => {
                            LogString += msg.GetString() + "\r\n";
                            while (LogString.Length > 1000) {
                                LogString = LogString.Substring(1);
                            }
                            //Log.Insert(0, logc);
                        });
                    }
                }
            } catch (Exception e) {
                Loging.Log.Erro(e.Message, e.StackTrace);
            }
        }
    }
}
