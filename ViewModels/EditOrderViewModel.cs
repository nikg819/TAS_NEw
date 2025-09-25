using System;
using ReactiveUI;
using System.Reactive;

namespace TAS_Test.ViewModels;

public class EditOrderViewModel : ReactiveObject
{
    private ReactiveObject _currentView;
    public ReactiveObject CurrentView
    {
        get => _currentView;
        set => this.RaiseAndSetIfChanged(ref _currentView, value);
    }
    public ReactiveCommand<Unit, Unit> UpdateOrder { get; }
    
    public EditOrderViewModel()
    {
        UpdateOrder = ReactiveCommand.Create(OrderSafeButton);
    }

    private void OrderSafeButton()
    {
        Console.WriteLine("Die Bestellung wird aktualisiert und es wird AllOrders geöffnet");
        CurrentView = new AllOrdersViewModel();
    }
}