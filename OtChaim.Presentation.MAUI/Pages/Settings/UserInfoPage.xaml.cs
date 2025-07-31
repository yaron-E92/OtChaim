using OtChaim.Presentation.MAUI.ViewModels.Settings;

namespace OtChaim.Presentation.MAUI.Pages.Settings;

public partial class UserInfoPage : ContentView
{
    public UserInfoPage(UserInfoViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
