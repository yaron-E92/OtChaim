using OtChaim.Presentation.MAUI.ViewModels.Tool;

namespace OtChaim.Presentation.MAUI.Pages.Tool;

public partial class ToolTabPage : ContentPage
{
    public ToolTabPage()
    {
        InitializeComponent();
        BindingContext = new ToolTabViewModel();
    }
}
