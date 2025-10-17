using ReactiveUI;

namespace TAS_Test.Models;

public class Customer : ReactiveObject
{
    public int k_id {get; set;}
    public string name {get; set;}
    public string fahrzeug {get; set;}
    public string mail {get; set;}
    public string phone {get; set;}
    public string? notes {get; set;}
    public Customer() { }
    
    public Customer(int k_id, string name, string fahrzeug, string mail, string phone, string? notes = "")
    {
        this.k_id = k_id;
        this.name = name ?? "";
        this.fahrzeug = fahrzeug ?? "";
        this.mail = mail ?? "";
        this.phone = phone ?? "";
        this.notes = notes ?? "";
    }
}

