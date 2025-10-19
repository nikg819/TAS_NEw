using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using ReactiveUI;
using TAS_Test.Models;

namespace TAS_Test.ViewModels;

public class ShowOrderViewModel : ReactiveObject
{
    public Action<ReactiveObject>? Navigate { get; set; }
    public string Header { get; set; } 
    public string Subheader { get; set; }
    public string ShowOrderOrderId { get; set; }
    public string ShowOrderKundenNamen { get; set; }
    public string ShowOrderFahrzeug { get; set; }
    public string ShowOrderMail { get; set; }
    public string ShowOrderPhone { get; set; }
    public string ShowOrderKundenbemerkungen { get; set; }
    public string ShowOrderMaximaleKosten { get; set; }
    public string ShowOrderReparaturen { get; set; }
    public string ShowOrderCreationDate { get; set; }
    public string ShowOrderAuftragsdatum { get; set; }
    public string ShowOrderLexwareId { get; set; }
    public string ShowOrderKennzeichen { get; set; }
    public string ShowOrderFModell { get; set; }
    public string ShowOrderFFarbe { get; set; }
    public string ShowOrderArtikelListe { get; set; }
    public string ShowOrderOrderNotes { get; set; }
    public string ShowOrderInvoiceCreated { get; set; }
    public string ShowOrderInProgressSince { get; set; }
    public string ShowOrderFinishedSince { get; set; }
    
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
        var db = new Database.Database();
        List<int> articleIdList = db.GetArticlesFromOrder(order.order_id);
        var list = string.Join(", ", articleIdList.Select(id => db.GetArticleById(id).ArticleName));
        
        Header = $"{order.auftragsnamen}";
        Subheader = $"Auftragsnummer: {order.order_id}";
        
        ShowOrderKundenNamen = $"{order.name}";
        ShowOrderMail = $"{order.mail}";
        ShowOrderPhone = $"{order.phone}";
        ShowOrderLexwareId = $"{order.lexwareId}";
        
        ShowOrderKennzeichen = $"{order.kennzeichen}";
        ShowOrderFModell = $"{order.vehicleModel}";
        ShowOrderFFarbe = $"{order.vehicleColour}";
        
        ShowOrderAuftragsdatum = $"{order.auftragsdatum}";
        ShowOrderArtikelListe = $"{list}";
        ShowOrderOrderNotes = $"{order.orderNotes}";
        ShowOrderMaximaleKosten = $"{order.maxKosten}";

        ShowOrderCreationDate = $"{order.creationDate}";
        ShowOrderInProgressSince = $"{order.inProgressSince}";
        ShowOrderInvoiceCreated = $"{order.invoiceCreatedDate}";
        ShowOrderFinishedSince = $"{order.orderFinishedDate}";
    }
}