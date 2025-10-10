using System;
using ReactiveUI;
using System.Reactive;
using TAS_Test.Models;

namespace TAS_Test.ViewModels;

public class EditOrderViewModel : ReactiveObject
{
    public Action<ReactiveObject>? Navigate { get; set; }
    
    public string Header { get; set; } = "";
    public string Subheader { get; set; } = "";
    public ReactiveCommand<Unit, Unit> UpdateOrder { get; }
    
    public string InputAuftragsnamen { get; set; }
    public string InputAuftragsdatum { get; set; }
    public string InputMaxKosten { get; set; }
    public string InputReparaturen { get; set; }

    public EditOrderViewModel(Order order)
    {
        Header = $"{order.auftragsnamen}";
        Subheader = $"Auftragsnummer: {order.order_id}";
        
        InputAuftragsnamen = order.auftragsnamen;
        InputAuftragsdatum = order.auftragsdatum;
        InputMaxKosten = order.maxKosten;
        InputReparaturen = order.reparaturen;
        
        UpdateOrder = ReactiveCommand.Create(() => OrderSafeButton(order));
    }

    private void OrderSafeButton(Order order)
    {
        var db = new Database.Database();
        db.UpdateOrder(order.order_id, InputAuftragsnamen, InputAuftragsdatum, InputMaxKosten, InputReparaturen);
        
        var allOrdersvm = new AllOrdersViewModel();
        allOrdersvm.CreateOrderlist();
        Console.WriteLine("Auftrag gespeichert und Open All Orders");
        allOrdersvm.Navigate = Navigate; // wieder weiterreichen
        Navigate?.Invoke(allOrdersvm);
    }
}