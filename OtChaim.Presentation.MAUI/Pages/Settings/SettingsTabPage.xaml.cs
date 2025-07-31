using OtChaim.Presentation.MAUI.ViewModels.Settings;

namespace OtChaim.Presentation.MAUI.Pages.Settings;

public partial class SettingsTabPage : ContentPage
{
    public SettingsTabPage()
    {
        InitializeComponent();
        BindingContext = new SettingsTabViewModel();
    }
}
