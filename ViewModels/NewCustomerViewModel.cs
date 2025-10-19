using System;
using System.Reactive;
using ReactiveUI;
using TAS_Test.Models;

namespace TAS_Test.ViewModels;

public class NewCustomerViewModel : ReactiveObject
{
    public Action<ReactiveObject>? Navigate { get; set; }
    public ReactiveCommand<Unit, Unit> ButtonAddNewCustomer { get; }
    
    public string InputName { get; set; }
    public string InputMail { get; set; }
    public string InputPhone { get; set; }
    public string InputNotes { get; set; }
    public string InputLexwareId { get; set; }
    
    private string _subheader = "";

    public string Subheader
    {
        get => _subheader;
        set => this.RaiseAndSetIfChanged(ref _subheader, value);
    }

    public NewCustomerViewModel()
    {
        ButtonAddNewCustomer = ReactiveCommand.Create(ButtonSafeNewCustomer);
    }

    //Speichert und prüft eingegebene Kundendaten
    private void ButtonSafeNewCustomer()
    {
        if (!string.IsNullOrWhiteSpace(InputName)) //prüft ob Name angegeben
        {
            //neuen Kunden anlegen
            Customer k = new Customer(
                1,
                InputName ?? "",
                InputLexwareId ?? "",
                InputMail ?? "",
                InputPhone ?? "",
                InputNotes ?? "");
            var db = new Database.Database();
            db.AddCustomer(k);

            var kundenlistevm = new KundenlisteViewModel();
            kundenlistevm.CreateCustomerlist();
            Console.WriteLine("Kunde gespeichert und Open Kundenliste");
            kundenlistevm.Navigate = Navigate; // wieder weiterreichen
            Navigate?.Invoke(kundenlistevm);
        }
        else
        {
            Subheader = "Kunde wurde nicht gespeichert. Name fehlt";
        }
    }

    

}