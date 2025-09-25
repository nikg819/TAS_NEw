using ReactiveUI;

namespace TAS_Test.ViewModels;

public class ShowOrderViewModel : ReactiveObject
{
    private ReactiveObject _currentView;
    public ReactiveObject CurrentView
    {
        get => _currentView;
        set => this.RaiseAndSetIfChanged(ref _currentView, value);
    }
    
    
    public ShowOrderViewModel()
    {
        
    }
    
}