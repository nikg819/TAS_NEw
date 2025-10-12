using System;
using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace TAS_Test.ViewModels;

public class InfoViewModel : ReactiveObject
{
    public string Message { get; }

    public ReactiveCommand<Unit, Unit> JaCommand { get; }
    public ReactiveCommand<Unit, Unit> NeinCommand { get; }
    
    private readonly Window _window;
    private readonly ReactiveObject _viewModel;

    public InfoViewModel(Window window, string message,int customerID, ReactiveObject viewModel)
    {
        _window = window;
        Message = message;
        _viewModel = viewModel;

        NeinCommand = ReactiveCommand.Create(() =>
        {
            _window.Close();
        });
        JaCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            if (_viewModel is KundenlisteViewModel kundenliste)
            {
                var db = new Database.Database();
                bool deleted = await db.DeleteCustomer(customerID);
                if (deleted)
                {
                    Console.WriteLine("Kunde wird gelöscht");
                    kundenliste.CreateCustomerlist();
                    _window.Close();
                }
                else
                {
                    kundenliste.Subheader = "Der Kunde kann nicht gelöscht werden, da er noch offene Aufträge hat.";
                    _window.Close();
                }
            }
            if (_viewModel is AllOrdersViewModel orderliste)
            {
                var db = new Database.Database();
                bool deleted = await db.DeleteOrder(customerID);
                if (deleted)
                {
                    Console.WriteLine("Auftrag wird gelöscht");
                    orderliste.CreateOrderlist();
                    _window.Close();
                }
                else
                {
                    orderliste.Subheader = "Der Auftrag kann nicht gelöscht werden";
                    _window.Close();
                }
            }
            
            
        });
    }
    
}