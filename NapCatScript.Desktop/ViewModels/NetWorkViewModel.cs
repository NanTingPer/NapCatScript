using System.Collections.ObjectModel;
using NapCatScript.Desktop.ViewModels.NetWorkModels;

namespace NapCatScript.Desktop.ViewModels;

public class NetWorkViewModel : ViewModelBase
{
    public ObservableCollection<HttpServerViewModel> HttpServers { get; set; } = [];

    public NetWorkViewModel()
    {
        for (int i = 0; i < 10; i++)
        {
            HttpServers.Add(new HttpServerViewModel());
        }
    }
}