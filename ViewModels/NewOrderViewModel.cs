using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using TAS_Test.Models;

namespace TAS_Test.ViewModels;

public class NewOrderViewModel : ReactiveObject
{
    public Action<ReactiveObject>? Navigate { get; set; }
    public ReactiveCommand<Unit, Unit> ButtonAddNewOrder { get; }
    
    private ObservableCollection<Article> _articleSelection = new();
    public ObservableCollection<Article> ArticleSelection
    {
        get => _articleSelection;
        set => this.RaiseAndSetIfChanged(ref _articleSelection, value);
    }
    public string ArtikelNamen {get; set;}
    public string InputAuftragsnamen { get; set; }
    public string InputAuftragsdatum { get; set; }
    public string InputOrderNotes{ get; set; }
    
    private string _inputKennzeichen;
    public string InputKennzeichen
    {
        get => _inputKennzeichen;
        set
        {
            this.RaiseAndSetIfChanged(ref _inputKennzeichen, value);
            kennzeichenFinden(value);
        }
    }
    private string _inputFahrzeugModell;
    public string InputFahrzeugModell
    {
        get => _inputFahrzeugModell;
        set => this.RaiseAndSetIfChanged(ref _inputFahrzeugModell, value);
    }

    private string _inputFahrzeugFarbe;
    public string InputFahrzeugFarbe
    {
        get => _inputFahrzeugFarbe;
        set => this.RaiseAndSetIfChanged(ref _inputFahrzeugFarbe, value);
    }

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
        ButtonAddNewOrder = ReactiveCommand.Create(() => ButtonSafeNewOrder(customer, InputKennzeichen, InputFahrzeugModell, InputFahrzeugFarbe));
        Header = $"Neuer Auftrag für {customer.name}";
        Subheader = $"Kundennummer: {customer.lexwareId}";
        
        CreateArticleList();
    }

    public void ButtonSafeNewOrder(Customer customer, string kennzeichen, string vModel, string vColour)
    {
        if (!string.IsNullOrWhiteSpace(kennzeichen) || !string.IsNullOrWhiteSpace(InputAuftragsnamen))
        {
            var db = new Database.Database();
            string configKennzeichen = kennzeichen.ToLower().Trim().Replace(" ", "");
            int vId = kennzeichenFinden(configKennzeichen);
            int orderVId = 0;
            var selectedArticles = ArticleSelection.Where(a => a.IsChecked).ToList();
            
            if (vId != 0)
            {
                db.UpdateVehicle(vId, configKennzeichen, vModel, vColour);
                orderVId = vId;
            }
            else if (vId == 0)
            {
                db.AddVehicle(configKennzeichen, vModel, vColour);
                orderVId = kennzeichenFinden(configKennzeichen);
            }
        
            DateTime time = DateTime.Now;
            string creationDate = time.ToString("dd.MM.yyyy 'um' HH:mm:ss");
        
            /*

             OrderID
             VehicleID -> zuerst muss vehicle in db gespeichert werden , dann wird id an add order übergeben
             Auftragsdatum
             maxKOsten
             status
             auftragsnamen
             kundenID
             ordernotes
             creationdate

            zukünftig: articleIDS

             */
        
            Order newOrder = new Order(
                1,
                orderVId,
                InputAuftragsdatum ?? "",
                InputMaximaleKosten ?? "",
                1,
                InputAuftragsnamen ?? "",
                customer.k_id,
                InputOrderNotes ?? "",
                creationDate
            );
        
            int newOrderId = db.AddOrder(newOrder);
            
            foreach (var item in selectedArticles)
                db.AddOrderArticle(newOrderId, item.ArticleDatabaseId);

            var kundenlistevm = new KundenlisteViewModel();
            kundenlistevm.CreateCustomerlist();
            kundenlistevm.Navigate = Navigate; 
            Navigate?.Invoke(kundenlistevm);
        }
        else
        {
            Subheader = "❌Auftrag nicht gespeichert. Name oder Kennzeichen fehlt";
        }
    }

    public void CreateArticleList()
    {
        var db = new Database.Database();
        var allArticles = db.GetAllArticles();
        ArticleSelection = new ObservableCollection<Article>(allArticles);
    }
    
    private int kennzeichenFinden(string kennzeichen)
    {
        string kennzeichenConfig = kennzeichen.ToLower().Trim().Replace(" ", "");
        
        var db = new Database.Database();
        Vehicle? vehicle = db.GetVehicleByKennzeichen(kennzeichenConfig);
        
        if (vehicle == null) //kein fahrzeug gefunden
        {
            InputFahrzeugModell = "";
            InputFahrzeugFarbe = "";
            return 0;
        }
        
        InputFahrzeugModell = vehicle.vehicleModel ?? "";
        InputFahrzeugFarbe = vehicle.vehicleColour ?? "";
        
        return vehicle.vehicleId;
    }
    

}