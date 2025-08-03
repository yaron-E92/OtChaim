using OtChaim.Presentation.MAUI.Pages.Tool;
using OtChaim.Presentation.MAUI.Pages.Settings;

namespace OtChaim.Presentation.MAUI;

/// <summary>
/// Represents the shell for the OtChaim MAUI application, defining navigation structure.
/// </summary>
public partial class AppShell : Shell
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppShell"/> class.
    /// Sets up navigation and routes.
    /// </summary>
    public AppShell()
    {
        InitializeComponent();

        // Register routes for tabbed navigation
        Routing.RegisterRoute(nameof(ToolTabPage), typeof(ToolTabPage));
        Routing.RegisterRoute(nameof(SettingsTabPage), typeof(SettingsTabPage));
    }
}
