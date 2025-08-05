using OtChaim.Presentation.MAUI.ViewModels.Settings;

namespace OtChaim.Presentation.MAUI.Pages.Settings;

public partial class EmergencyContactsPage : ContentView
{
    public EmergencyContactsPage(EmergencyContactsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
