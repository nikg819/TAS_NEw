using System;
using System.Collections.ObjectModel;
using System.Reactive;
using ReactiveUI;
using TAS_Test.Models;

namespace TAS_Test.ViewModels;

public class AllOrdersViewModel : ReactiveObject
{
    public Action<ReactiveObject>? Navigate { get; set; }
    private ObservableCollection<Order> _allOrders = new();
    public ObservableCollection<Order> AllOrders
    {
        get => _allOrders;
        set => this.RaiseAndSetIfChanged(ref _allOrders, value);
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
    public ReactiveCommand<string, Unit> SearchCommand { get; }
    public ReactiveCommand<Order, Unit> ErledigtCommand { get; }
    public ReactiveCommand<Order, Unit> EditCommand { get; }
    public ReactiveCommand<Order, Unit> DeleteCommand { get; }
    public ReactiveCommand<Order, Unit> ShowCommand { get; }
    public ReactiveCommand<Order, Unit> PrintCommand { get; }
    
    public AllOrdersViewModel()
    {
        SearchCommand = ReactiveCommand.Create<string>(SearchOrder);
        ErledigtCommand = ReactiveCommand.Create<Order>(ErledigtOrder);
        EditCommand = ReactiveCommand.Create<Order>(EditOrder);
        DeleteCommand = ReactiveCommand.Create<Order>(DeleteOrder);
        ShowCommand = ReactiveCommand.Create<Order>(ShowOrder);
        PrintCommand = ReactiveCommand.Create<Order>(PrintOrder);
        
        CreateOrderlist();
    }

    private void SearchOrder(string searchText)
    {
        if (!string.IsNullOrEmpty(searchText))
        {
            var db = new Database.Database();
            var searchList = db.FindOrderBySearch(searchText);
            Subheader = $"Anzahl Aufträge: {searchList.Count}";
            AllOrders = new ObservableCollection<Order>(searchList);
            SearchText = "";
        }
        else
        {
            SearchText = "";
            CreateOrderlist();
        }
    }

    private void ErledigtOrder(Order order)
    {
        Console.WriteLine("ErledigtOrder");
    }

    private void EditOrder(Order order)
    {
        Console.WriteLine($"Edit {order.auftragsnamen}");
    }
    
    private void DeleteOrder(Order order)
    {
        Console.WriteLine($"Delete {order.auftragsnamen}");
    }

    private void ShowOrder(Order order)
    {
        Console.WriteLine($"Show {order.auftragsnamen}");
    }

    private void PrintOrder(Order order)
    {
        Console.WriteLine($"Print {order.auftragsnamen}");
    }

    public void CreateOrderlist()
    {
        Console.WriteLine("Create Orderlist");
        var db = new Database.Database();
        var allOrders = db.GetAllOrders();
        Subheader = $"Anzahl Aufträge: {allOrders.Count}";
        AllOrders = new ObservableCollection<Order>(allOrders);
    }
    
    
}