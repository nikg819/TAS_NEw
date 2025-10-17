using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading;
using ReactiveUI;
using TAS_Test.Models;
using TAS_Test.services;
using TAS_Test.Views;

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
    
    private bool _isAscending = true;
    public bool IsAscending
    {
        get => _isAscending;
        set
        {
            if (_isAscending == value) return; 
            this.RaiseAndSetIfChanged(ref _isAscending, value);
            CreateOrderlist();
            this.RaisePropertyChanged(nameof(IsDescending)); 
        }
    }

    public bool IsDescending
    {
        get => !_isAscending;
        set
        {
            IsAscending = !value; // nur IsAscending setzen
        }
    }
    
    public string SortOrder => IsAscending ? "ASC" : "DESC";
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
        var db = new Database.Database();
        db.ChangeStatus(order);
        CreateOrderlist();
    }

    private void EditOrder(Order order)
    {
        var editOrdervm = new EditOrderViewModel(order);
        editOrdervm.Navigate = Navigate;
        Navigate?.Invoke(editOrdervm);
    }
    
    private void DeleteOrder(Order order)
    {
        string message = $"Willst du {order.auftragsnamen} löschen?";
        var infowindow = new InfoWindow();
        infowindow.DataContext = new InfoViewModel(infowindow, message, order.order_id,this);
        infowindow.Show();
    }

    private void ShowOrder(Order order)
    {
        var showordervm = new ShowOrderViewModel(order);
        showordervm.Navigate = Navigate;
        Navigate?.Invoke(showordervm);
    }

    private void PrintOrder(Order order)
    {
        var pdf = new PdfService();
        string fileName = order.auftragsnamen.Replace(" ", "_");
        Console.WriteLine(fileName);
        try
        {
            pdf.HtmlToPdf(orderTemplate.html(order),
                $"{fileName}_{order.order_id}.pdf");
            Subheader = $"✅ PDF '{fileName}_{order.order_id}.pdf' erfolgreich erstellt";
        }
        catch (Exception e)
        {
            Subheader = $"Es ist ein Fehler aufgetreten: {e.Message}";
        }
        
    }
    
    public void CreateOrderlist()
    {
        var db = new Database.Database();
        var allOrders = db.GetAllOrders(SortOrder);
        Subheader = $"Anzahl Aufträge: {allOrders.Count}";
        AllOrders = new ObservableCollection<Order>(allOrders);
    }
    
    
}