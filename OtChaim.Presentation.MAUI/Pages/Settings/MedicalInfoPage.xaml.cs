using OtChaim.Presentation.MAUI.ViewModels.Settings;

namespace OtChaim.Presentation.MAUI.Pages.Settings;

public partial class MedicalInfoPage : ContentView
{
    public MedicalInfoPage(MedicalInfoViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
