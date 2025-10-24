using System;
using ReactiveUI;
using System.Reactive;
using TAS_Test.Models;

namespace TAS_Test.ViewModels;

public class EditCustomerViewModel : ReactiveObject
{
    public Action<ReactiveObject>? Navigate { get; set; }
    public string Header { get; set; } = "";
    public string Subheader { get; set; } = "";
    private string _subheader2 = "";

    public string Subheader2
    {
        get => _subheader2;
        set => this.RaiseAndSetIfChanged(ref _subheader2, value);
    }

    public string InputName { get; set; }
    public string InputLexwareId { get; set; }
    public string InputMail { get; set; }
    public string InputPhone { get; set; }
    public string InputNotes { get; set; }
    public ReactiveCommand<Unit, Unit> ButtonSafeCustomer { get; }
    
    public EditCustomerViewModel(Customer customer)
    {
        Header = $"{customer.name}";
        Subheader = $"Kundennummer: {customer.k_id}";
        
        InputName = customer.name;
        InputLexwareId = customer.lexwareId;
        InputMail = customer.mail;
        InputPhone = customer.phone;
        InputNotes = customer.notes;
        
        ButtonSafeCustomer = ReactiveCommand.Create(() => ButtonUpdateCustomer(customer));;
    }

    private void ButtonUpdateCustomer(Customer customer)
    {
        if (!string.IsNullOrWhiteSpace(InputName))
        {
            var db = new Database.Database();
            db.UpdateCustomer(customer.k_id, InputName, InputLexwareId, InputMail, InputPhone, InputNotes);
        
            var kundenlistevm = new KundenlisteViewModel();
            kundenlistevm.CreateCustomerlist();
            Console.WriteLine("Kunde gespeichert und Open Kundenliste");
            kundenlistevm.Navigate = Navigate; // wieder weiterreichen
            Navigate?.Invoke(kundenlistevm);
        }
        else
        {
            Subheader2 = "❌ Kunde wurde nicht gespeichert. Name fehlt";
        }
        
    }

    
}