using OtChaim.Presentation.MAUI.ViewModels.Tool;

namespace OtChaim.Presentation.MAUI.Pages.Tool;

public partial class EmergencyDashboardPage : ContentView
{
    public EmergencyDashboardPage(EmergencyDashboardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
