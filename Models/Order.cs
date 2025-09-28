namespace TAS_Test.Models;


public class Order
{
    public int order_id {get; set;}
    public string auftragsdatum {get; set;}
    public string maxKosten {get; set;}
    public string status {get; set;}
    public string auftragsnamen {get; set;}
    public int k_id {get; set;}
    public string name { get; set; }
    public string fahrzeug { get; set; }

    public Order() { }
    
    public Order(int order_id, string auftragsdatum, string maxKosten, string status, string auftragsnamen, int k_id, string name, string fahrzeug)
    {
        this.order_id = order_id;
        this.auftragsdatum = auftragsdatum;
        this.maxKosten = maxKosten;
        this.status = status;
        this.auftragsnamen = auftragsnamen;
        this.k_id = k_id;
        this.name = name;
        this.fahrzeug = fahrzeug;
    }
}


