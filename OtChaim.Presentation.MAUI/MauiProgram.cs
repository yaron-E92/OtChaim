using Microsoft.Extensions.Logging;
using OtChaim.Application.Common;
using OtChaim.Persistence;
using OtChaim.Presentation.MAUI.ViewModels;
using OtChaim.Presentation.MAUI.Services;

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

        // Register view models
        builder.Services.AddTransient<EmergencyDashboardViewModel>();

        // Register pages
        builder.Services.AddTransient<MainPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
