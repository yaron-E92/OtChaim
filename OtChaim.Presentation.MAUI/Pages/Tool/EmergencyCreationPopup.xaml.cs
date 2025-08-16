using OtChaim.Presentation.MAUI.ViewModels.Tool;

namespace OtChaim.Presentation.MAUI.Pages.Tool;

public partial class EmergencyCreationPopup : ContentView
{
    public EmergencyCreationPopup(EmergencyCreationViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
