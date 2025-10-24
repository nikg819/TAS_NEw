using System;
using System.Collections.ObjectModel;
using ReactiveUI;
using System.Reactive;
using TAS_Test.Models;

namespace TAS_Test.ViewModels;

public class OrderArchiveViewModel : ReactiveObject
{
    private ObservableCollection<Order> _allOrders = new();
    public ObservableCollection<Order> AllArchiveOrders
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
    public ReactiveCommand<string, Unit> ArchiveSearchCommand { get; }
    public ReactiveCommand<Order, Unit> ReactivateCommand { get; }
    
    public OrderArchiveViewModel()
    {
        ArchiveSearchCommand = ReactiveCommand.Create<string>(SearchOrder);
        ReactivateCommand = ReactiveCommand.Create<Order>(ReactivateOrder);
        
        CreateArchiveOrderlist();
    }

    private void SearchOrder(string searchText)
    {
        if (!string.IsNullOrEmpty(searchText))
        {
            var db = new Database.Database();
            var archiveSearchList = db.FindArchiveOrderBySearch(searchText);
            Subheader = $"Anzahl Aufträge: {archiveSearchList.Count}";
            AllArchiveOrders = new ObservableCollection<Order>(archiveSearchList);
            SearchText = "";
        }
        else
        {
            SearchText = "";
            CreateArchiveOrderlist();
        }
    }

    private void ReactivateOrder(Order order)
    {
        var db = new Database.Database();
        db.ChangeStatus(order, 3);
        CreateArchiveOrderlist();
    }
    
    public void CreateArchiveOrderlist()
    {
        Console.WriteLine("Create ArchiveOrderlist");
        var db = new Database.Database();
        var allOrders = db.GetAllArchiveOrders();
        Subheader = $"Anzahl Aufträge: {allOrders.Count}";
        AllArchiveOrders = new ObservableCollection<Order>(allOrders);
    }
}