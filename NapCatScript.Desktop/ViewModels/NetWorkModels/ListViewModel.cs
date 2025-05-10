using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;
using NapCatScript.Core.JsonFormat;
using NapCatScript.Core.NetWork.NetWorkModel;

namespace NapCatScript.Desktop.ViewModels.NetWorkModels;

public class ListViewModel : ViewModelBase
{
    public ObservableCollection<object> NetWorkConfig { get; set; } = [];

    public ListViewModel()
    {
        SetConifg();
        for (int i = 0; i < 10; i++)
        {
            NetWorkConfig.Add(new HttpServerViewModel());
        }
    }

    public async void SetConifg()
    {
        string s = await NapCatScript.Core.MsgHandle.Utils.GetNetWorkConfig("6099", "napcat");
        if(!s.GetJsonElement(out var netWorkJson))
            return;
        if(!netWorkJson.TryGetPropertyValue("network", out var network))
            return;

        NetWorks config = network.Deserialize<NetWorks>()!;
    }

}