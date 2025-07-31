using Microsoft.Extensions.Logging;
using OtChaim.Application.Common;
using OtChaim.Persistence;
using OtChaim.Presentation.MAUI.ViewModels;
using OtChaim.Presentation.MAUI.Services;
using OtChaim.Presentation.MAUI.Pages.Tool;
using OtChaim.Presentation.MAUI.Pages.Settings;
using OtChaim.Presentation.MAUI.ViewModels.Settings;
using OtChaim.Presentation.MAUI.ViewModels.Tool;
using OtChaim.Presentation.MAUI.Abstractions;

namespace OtChaim.Presentation.MAUI;

/// <summary>
/// Provides configuration and startup logic for the OtChaim MAUI application.
/// </summary>
public static class MauiProgram
{
    /// <summary>
    /// Creates and configures the MAUI application.
    /// </summary>
    /// <returns>The configured <see cref="MauiApp"/> instance.</returns>
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register services
        builder.Services.AddPersistence(useInMemory: true);
        builder.Services.AddEventAggregator();

        // Register application services
        builder.Services.AddTransient<EmergencyDataService>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();

        // Register view models
        builder.Services.AddTransient<EmergencyDashboardViewModel>();
        builder.Services.AddTransient<ToolTabViewModel>();
        builder.Services.AddTransient<SettingsTabViewModel>();
        builder.Services.AddTransient<EmergencyViewModel>();
        builder.Services.AddTransient<GroupStatusViewModel>();
        builder.Services.AddTransient<UserInfoViewModel>();
        builder.Services.AddTransient<MedicalInfoViewModel>();
        builder.Services.AddTransient<EmergencyContactsViewModel>();

        // Register pages
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<ToolTabPage>();
        builder.Services.AddTransient<SettingsTabPage>();
        builder.Services.AddTransient<EmergencyPage>();
        builder.Services.AddTransient<GroupStatusPage>();
        builder.Services.AddTransient<UserInfoPage>();
        builder.Services.AddTransient<MedicalInfoPage>();
        builder.Services.AddTransient<EmergencyContactsPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
