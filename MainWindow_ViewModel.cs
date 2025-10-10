using System;
using ReactiveUI;
using System.Reactive;
using TAS_Test.ViewModels;

namespace TAS_Test;

public class MainWindow_ViewModel : ReactiveObject
{
    private ReactiveObject _currentView;
    private readonly Action<ReactiveObject>? _navigateAction;

    public ReactiveObject CurrentView
    {
        get => _currentView;
        set => this.RaiseAndSetIfChanged(ref _currentView, value);
    }
    public ReactiveCommand<Unit, Unit> Kundenliste { get; }
    public ReactiveCommand<Unit, Unit> Aufträge { get; }
    public ReactiveCommand<Unit, Unit> Archiv { get; }
    public ReactiveCommand<Unit, Unit> Einstellungen { get; }
    
    public MainWindow_ViewModel()
    {
        _navigateAction = vm => CurrentView = vm;
        
        Kundenliste = ReactiveCommand.Create(OpenCustomerlist);
        Aufträge = ReactiveCommand.Create(OpenOrders);
        Archiv = ReactiveCommand.Create(OpenArchive);
        Einstellungen = ReactiveCommand.Create(OpenSettings);
        TASStart();
        OpenCustomerlist();
    }

    private void TASStart()
    {
        var db = new Database.Database();
        if (!db.TestConnection())
        {
            Console.WriteLine("TestConnection failed");
            Environment.Exit(1);
        }
    }
    public void OpenCustomerlist()
    {
        var kundenlistevm = new KundenlisteViewModel();
        kundenlistevm.Navigate = _navigateAction;
        CurrentView = kundenlistevm;
    }
    
    public void OpenOrders()
    {
        var ordersVm = new AllOrdersViewModel();
        ordersVm.Navigate = _navigateAction;
        CurrentView = ordersVm;
    }
    private void OpenArchive()
    {
        Console.WriteLine("Open Archiv");
        CurrentView = new OrderArchiveViewModel();
    }
    private void OpenSettings()
    {
        Console.WriteLine("Open Einstellungen, mehr gibt es hier nicht");
    }
    
    

    

    

}