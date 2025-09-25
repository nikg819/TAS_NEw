using System;
using ReactiveUI;
using System.Reactive;

namespace TAS_Test.ViewModels;

public class OrderArchiveViewModel : ReactiveObject
{
    private ReactiveObject _currentView;
    public ReactiveObject CurrentView
    {
        get => _currentView;
        set => this.RaiseAndSetIfChanged(ref _currentView, value);
    }
    public ReactiveCommand<Unit, Unit> EnterSearchArchive { get; }
    
    public OrderArchiveViewModel()
    {
        EnterSearchArchive = ReactiveCommand.Create(DoSearchArchive);
    }

    private void DoSearchArchive()
    {
        Console.WriteLine("Search in Archive");
    }
}