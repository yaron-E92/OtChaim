using OtChaim.Presentation.MAUI.ViewModels;

namespace OtChaim.Presentation.MAUI;

/// <summary>
/// Represents the main page of the OtChaim MAUI application.
/// </summary>
public partial class MainPage : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MainPage"/> class.
    /// Sets the binding context to the provided view model.
    /// </summary>
    /// <param name="viewModel">The emergency dashboard view model.</param>
    public MainPage(EmergencyDashboardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
