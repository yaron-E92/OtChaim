using OtChaim.Presentation.MAUI.ViewModels.Tool;

namespace OtChaim.Presentation.MAUI.Pages.Tool;

public partial class GroupStatusPage : ContentView
{
    public GroupStatusPage()
    {
        InitializeComponent();
        BindingContext = new GroupStatusViewModel();
    }
}
