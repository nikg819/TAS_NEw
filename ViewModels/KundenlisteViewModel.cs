using System;
using ReactiveUI;
using System.Reactive;

namespace TAS_Test.ViewModels;

public class KundenlisteViewModel : ReactiveObject
{
    public Action<ReactiveObject>? Navigate { get; set; }
    public ReactiveCommand<Unit, Unit> EnterSearchCustomer { get; }
    public ReactiveCommand<Unit, Unit> NewCustomer { get; }
    
    
    public KundenlisteViewModel()
    {
        EnterSearchCustomer = ReactiveCommand.Create(DoSearchCustomer);
        NewCustomer = ReactiveCommand.Create(NewCustomerSafeButton);
    }

    private void NewCustomerSafeButton()
    {
        Console.WriteLine("Open Customer Input");
        var newCustomer = new NewCustomerViewModel();
        newCustomer.Navigate = Navigate; // Callback weiterreichen
        Navigate?.Invoke(newCustomer);
    }

    private void DoSearchCustomer()
    {
        Console.WriteLine("Auf der Suche");
    }
    
}