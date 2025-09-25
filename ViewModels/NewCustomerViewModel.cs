using System;
using System.Reactive;
using ReactiveUI;

namespace TAS_Test.ViewModels;

public class NewCustomerViewModel : ReactiveObject
{
    public Action<ReactiveObject>? Navigate { get; set; }
    public ReactiveCommand<Unit, Unit> ButtonAddNewCustomer { get; }
    
    public NewCustomerViewModel()
    {
        ButtonAddNewCustomer = ReactiveCommand.Create(ButtonSafeNewCustomer);
    }
    
    private void ButtonSafeNewCustomer()
    {
        Console.WriteLine("Zurück zur Kundenliste");
        var kundenliste = new KundenlisteViewModel();
        kundenliste.Navigate = Navigate; // wieder weiterreichen
        Navigate?.Invoke(kundenliste);
    }
    
}