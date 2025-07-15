namespace OtChaim.Presentation.MAUI;

/// <summary>
/// Represents the main application class for OtChaim MAUI.
/// </summary>
public partial class App : Microsoft.Maui.Controls.Application
{
    /// <summary>
    /// Initializes a new instance of the <see cref="App"/> class.
    /// Sets up the main page.
    /// </summary>
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }
}
