using Avalonia.Controls;
using TAS_Test.ViewModels;

namespace TAS_Test.Views;

public partial class Kundenliste : UserControl
{
    public Kundenliste()
    {
        InitializeComponent();
        DataContext = new KundenlisteViewModel();

    }
}