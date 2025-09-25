using Avalonia.Controls;

namespace TAS_Test;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindow_ViewModel();
    }
}