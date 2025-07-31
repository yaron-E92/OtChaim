using OtChaim.Presentation.MAUI.ViewModels.Tool;

namespace OtChaim.Presentation.MAUI.Pages.Tool;

public partial class EmergencyPage : ContentView
{
    public EmergencyPage()
    {
        InitializeComponent();
        BindingContext = new EmergencyViewModel();
    }
}
