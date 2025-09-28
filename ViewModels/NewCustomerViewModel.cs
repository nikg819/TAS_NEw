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
    public string InputFahrzeug { get; set; }
    
    public string InputMail { get; set; }
    public string InputPhone { get; set; }
    public string InputNotes { get; set; }
    
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
    
    private void ButtonSafeNewCustomer()
    {
        AddNewCustomer();
        Console.WriteLine("Zurück zur Kundenliste");
        var kundenliste = new KundenlisteViewModel();
        kundenliste.Navigate = Navigate; // wieder weiterreichen
        Navigate?.Invoke(kundenliste);
    }
    private void AddNewCustomer()
    {
        
        if (!string.IsNullOrWhiteSpace(InputName))//prüft ob Name angegeben
        { 
            //neuen Kunden anlegen
            Customer k = new Customer(
                1, 
                InputName ?? "", 
                InputFahrzeug ?? "", 
                InputMail ?? "", 
                InputPhone ?? "", 
                InputNotes ?? "");
            var db = new Database.Database();
            db.AddCustomer(k);
            
            var kundenlistevm = new KundenlisteViewModel();
            kundenlistevm.CreateCustomerlist();
        }
        else
        {
            Subheader = "Kunde wurde nicht gespeichert. Name fehlt";
            
        }
    } 
    
}