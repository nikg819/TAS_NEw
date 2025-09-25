using System;
using ReactiveUI;
using System.Reactive;

namespace TAS_Test.ViewModels;

public class EditCustomerViewModel : ReactiveObject
{
    private ReactiveObject _currentView;
    public ReactiveObject CurrentView
    {
        get => _currentView;
        set => this.RaiseAndSetIfChanged(ref _currentView, value);
    }
    public ReactiveCommand<Unit, Unit> ButtonSafeCustomer { get; }
    
    public EditCustomerViewModel()
    {
        ButtonSafeCustomer = ReactiveCommand.Create(ButtonUpdateCustomer);
    }

    private void ButtonUpdateCustomer()
    {
        Console.WriteLine("update Customer");
        CurrentView = new KundenlisteViewModel();
    }
}