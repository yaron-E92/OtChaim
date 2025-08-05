using OtChaim.Presentation.MAUI.ViewModels.Tool;

namespace OtChaim.Presentation.MAUI.Pages.Tool;

public partial class EmergencyPage : ContentView
{
    public EmergencyPage(EmergencyViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
