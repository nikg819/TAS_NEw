using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace TAS_Test;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // ReactiveUI für Avalonia konfigurieren
        RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
    
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindow_ViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}