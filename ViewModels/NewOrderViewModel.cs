using System;
using System.Reactive;
using ReactiveUI;
using TAS_Test.Models;

namespace TAS_Test.ViewModels;

public class NewOrderViewModel : ReactiveObject
{
    public Action<ReactiveObject>? Navigate { get; set; }
    public ReactiveCommand<Unit, Unit> ButtonAddNewOrder { get; }
    
    public string InputAuftragsnamen { get; set; }
    public string InputAuftragsdatum { get; set; }
    public string InputReparaturen { get; set; }
    public string InputMaximaleKosten { get; set; }
    
    private string _header = "";

    public string Header
    {
        get => _header;
        set => this.RaiseAndSetIfChanged(ref _header, value);
    }
    
    private string _subheader = "";

    public string Subheader
    {
        get => _subheader;
        set => this.RaiseAndSetIfChanged(ref _subheader, value);
    }

    public NewOrderViewModel(Customer customer)
    {
        ButtonAddNewOrder = ReactiveCommand.Create(() => ButtonSafeNewOrder(customer));
        Header = $"Neuer Auftrag für {customer.name}";
    }

    public void ButtonSafeNewOrder(Customer customer)
    {
        Order newOrder = new Order(
            1,
            InputAuftragsdatum ?? "",
            InputMaximaleKosten ?? "",
            "1",
            InputAuftragsnamen,
            customer.k_id,
            InputReparaturen ?? "");
        var db = new Database.Database();
        db.AddOrder(newOrder);

        var kundenlistevm = new KundenlisteViewModel();
        kundenlistevm.CreateCustomerlist();
        Console.WriteLine("Kunde gespeichert und Open Kundenliste");
        kundenlistevm.Navigate = Navigate; 
        Navigate?.Invoke(kundenlistevm);
    }
    

}