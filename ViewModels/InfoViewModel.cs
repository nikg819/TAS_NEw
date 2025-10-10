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
    private readonly KundenlisteViewModel _kundenlistevm;

    public InfoViewModel(Window window, string message,int customerID, KundenlisteViewModel kundenlistevm)
    {
        _window = window;
        Message = message;
        _kundenlistevm = kundenlistevm;

        NeinCommand = ReactiveCommand.Create(() =>
        {
            _window.Close();
        });
        JaCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var db = new Database.Database();
            bool deleted = await db.DeleteCustomer(customerID);
            if (deleted)
            {
                Console.WriteLine("Kunde wird gelöscht");
                _kundenlistevm.CreateCustomerlist();
                _window.Close();
            }
            else
            {
                _kundenlistevm.Subheader = "Der Kunde kann nicht gelöscht werden, da er noch offene Aufträge hat.";
                _window.Close();
            }
            
        });
    }
    
}