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

    public InfoViewModel(Window window, string message,int customerID)
    {
        _window = window;
        Message = message;

        NeinCommand = ReactiveCommand.Create(() =>
        {
            _window.Close(); // Fenster schließen
        });
        JaCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var db = new Database.Database();
            await db.DeleteCustomer(customerID);
            Console.WriteLine("Kunde wird gelöscht");
            _window.Close();
            var kundenlistevm = new KundenlisteViewModel();
            Console.WriteLine("Kundenliste gekunde");
            kundenlistevm.CreateCustomerlist();
        });
    }

    
}