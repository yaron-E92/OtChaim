using OtChaim.Presentation.MAUI.ViewModels.Settings;

namespace OtChaim.Presentation.MAUI.Pages.Settings;

public partial class UserInfoPage : ContentView
{
    public UserInfoPage()
    {
        InitializeComponent();
        BindingContext = new UserInfoViewModel();
    }
}
