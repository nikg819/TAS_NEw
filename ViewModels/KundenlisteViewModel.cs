using System;
using System.Collections.ObjectModel;
using ReactiveUI;
using System.Reactive;
using TAS_Test.Models;

namespace TAS_Test.ViewModels;

public class KundenlisteViewModel : ReactiveObject
{
    //Navigation
    public Action<ReactiveObject>? Navigate { get; set; }

    // Commands
    public ReactiveCommand<string, Unit> SearchCommand { get; }
    public ReactiveCommand<Unit, Unit> NewCustomer { get; }
    public ReactiveCommand<Customer, Unit> EditCommand { get; }
    public ReactiveCommand<Customer, Unit> DeleteCommand { get; }
    public ReactiveCommand<Customer, Unit> NewOrderCommand { get; }

    // Collections + Properties
    private ObservableCollection<Customer> _allCustomers = new();
    public ObservableCollection<Customer> AllCustomers
    {
        get => _allCustomers;
        set => this.RaiseAndSetIfChanged(ref _allCustomers, value);
    }

    private string _subheader = "";
    public string Subheader
    {
        get => _subheader;
        set => this.RaiseAndSetIfChanged(ref _subheader, value);
    }
    
    private string _searchText;

    public string SearchText
    {
        get => _searchText;
        set => this.RaiseAndSetIfChanged(ref _searchText, value);
    }
    
    //View Konstruktor
    public KundenlisteViewModel()
    {
        SearchCommand = ReactiveCommand.Create<string>(SearchCustomer);
        NewCustomer = ReactiveCommand.Create(NewCustomerSafeButton);
        EditCommand = ReactiveCommand.Create<Customer>(EditCustomer);
        DeleteCommand = ReactiveCommand.Create<Customer>(DeleteCustomer);
        NewOrderCommand = ReactiveCommand.Create<Customer>(NewOrder);
        
        CreateCustomerlist();
    }

    // Methoden
    private void NewCustomerSafeButton()
    {
        Console.WriteLine("Open Customer Input");
        var newCustomer = new NewCustomerViewModel(){};
        newCustomer.Navigate = Navigate; // Callback weiterreichen
        Navigate?.Invoke(newCustomer);
    }

    private void SearchCustomer(string searchText)
    {
        Console.WriteLine($"Text:{searchText}");
    }

    private void EditCustomer(Customer customer)
    {
        Console.WriteLine($"Edit {customer.name}");
    }

    private void DeleteCustomer(Customer customer)
    {
        Console.WriteLine($"Delete {customer.name}");
    }

    private void NewOrder(Customer customer)
    {
        Console.WriteLine($"New Order for {customer.name}");
    }

    public void CreateCustomerlist()
    {
        var db = new Database.Database();
        var allPersons = db.GetAllPersons();
        Subheader = $"Anzahl Personen: {allPersons.Count}";
        AllCustomers = new ObservableCollection<Customer>(allPersons);
    }
}
