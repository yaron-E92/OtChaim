using OtChaim.Presentation.MAUI.ViewModels;

namespace OtChaim.Presentation.MAUI;

public partial class MainPage : ContentPage
{
    public MainPage(EmergencyDashboardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
