using System;
using System.Reactive;
using ReactiveUI;
using TAS_Test.Models;

namespace TAS_Test.ViewModels;

public class ShowOrderViewModel : ReactiveObject
{
    public Action<ReactiveObject>? Navigate { get; set; }
    public string Header { get; set; } 
    public string ShowOrderOrderId { get; set; }
    public string ShowOrderKundenNamen { get; set; }
    public string ShowOrderFahrzeug { get; set; }
    public string ShowOrderMail { get; set; }
    public string ShowOrderPhone { get; set; }
    public string ShowOrderKundenbemerkungen { get; set; }
    public string ShowOrderMaximaleKosten { get; set; }
    public string ShowOrderReparaturen { get; set; }
    public string ShowOrderTimestamp { get; set; }
    public string ShowOrderAuftragsdatum { get; set; }
    
    public ReactiveCommand<Unit, Unit> GoBack { get; }
    
    public ShowOrderViewModel(Order order)
    {
        GoBack = ReactiveCommand.Create(NavigateToAllOrders);

        ShowOrderData(order);
    }

    private void NavigateToAllOrders()
    {
        var allordersvm = new AllOrdersViewModel();
        allordersvm.Navigate = Navigate;
        Navigate?.Invoke(allordersvm);
    }

    private void ShowOrderData(Order order)
    {
        Header = $"{order.auftragsnamen}";
        
        ShowOrderOrderId = $"{order.order_id}";
        ShowOrderKundenNamen = $"{order.name}";
        ShowOrderFahrzeug = $"{order.fahrzeug}";
        ShowOrderMail = $"{order.mail}";
        ShowOrderPhone = $"{order.phone}";
        ShowOrderKundenbemerkungen = $"{order.kundenbemerkungen}";
        ShowOrderMaximaleKosten = $"{order.maxKosten}";
        ShowOrderReparaturen = $"{order.reparaturen}";
        ShowOrderTimestamp = $"{order.timestamp}";
        ShowOrderAuftragsdatum = $"{order.auftragsdatum}";
    }
}