using ReactiveUI;

namespace NapCatScript.Desktop.ViewModels.ChatViewModels;

public class ChatRightViewModel : ViewModelBase
{
    private ViewModelBase? _chatSocketSelectedViewModel;
    private ViewModelBase _currentViewModel;
    private ViewModelBase This => this;
    private ViewModelBase ChatSocketSelectedViewModel => _chatSocketSelectedViewModel ??= new ChatSocketSelectedViewModel();

    public ViewModelBase CurrentViewModel { get => _currentViewModel; set => this.RaiseAndSetIfChanged(ref _currentViewModel, value); }
     
    public ChatRightViewModel()
    {
        CurrentViewModel = ChatSocketSelectedViewModel;
    }
}