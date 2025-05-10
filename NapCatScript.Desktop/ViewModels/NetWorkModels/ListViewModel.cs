using System.Collections.ObjectModel;

namespace NapCatScript.Desktop.ViewModels.NetWorkModels;

public class ListViewModel : ViewModelBase
{
    public ObservableCollection<object> NetWorkConfig { get; set; } = [];

    public ListViewModel()
    {
        for (int i = 0; i < 10; i++)
        {
            NetWorkConfig.Add(new HttpServerViewModel());
        }
    }

}