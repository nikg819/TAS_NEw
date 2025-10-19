using Avalonia.Media;
using ReactiveUI;

namespace TAS_Test.Models;

public class Order : ReactiveObject
{
    public int order_id {get; set;}
    public int vehicleId {get; set;}
    public string auftragsdatum {get; set;}
    public string maxKosten {get; set;}
    public string auftragsnamen {get; set;}
    public int k_id {get; set;}
    public string name { get; set; }
    public string lexwareId { get; set; }
    public string mail { get; set; }
    public string phone { get; set; }
    public string orderNotes { get; set; }
    public string kundenbemerkungen { get; set; }
    public string creationDate {get; set;}
    public string inProgressSince {get; set;}
    public string vehicleModel {get; set;}
    public string vehicleColour {get; set;}
    public string kennzeichen {get; set;}
    public string invoiceCreatedDate {get; set;}
    public string orderFinishedDate {get; set;}
    private int _status;
    public int status
    {
        get => _status;
        set
        {
            this.RaiseAndSetIfChanged(ref _status, value);
            this.RaisePropertyChanged(nameof(DotColour));
        }
    }
    public SolidColorBrush DotColour =>
        status switch
        {
            1 => new SolidColorBrush(Colors.LightGreen),
            2 => new SolidColorBrush(Colors.Orange),
            3 => new SolidColorBrush(Colors.Red),
            _ => new SolidColorBrush(Colors.Gray)
        };

    public Order() { }
    
    public Order(int order_id, 
                int vehicleId, 
                string auftragsdatum, 
                string maxKosten, 
                int status, 
                string auftragsnamen, 
                int k_id, 
                string orderNotes, 
                string creationDate)
    {
        this.order_id = order_id;
        this.vehicleId = vehicleId; //FK Fahrezeug
        this.auftragsdatum = auftragsdatum;
        this.maxKosten = maxKosten;
        this.status = status;
        this.auftragsnamen = auftragsnamen;
        this.k_id = k_id; //FK Kunde
        this.orderNotes = orderNotes;
        this.creationDate = creationDate;
    }
}


