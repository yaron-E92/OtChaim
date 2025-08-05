using OtChaim.Presentation.MAUI.Abstractions;

namespace OtChaim.Presentation.MAUI.Services;

/// <summary>
/// Implementation of the navigation service using Shell navigation.
/// </summary>
public class NavigationService : INavigationService
{
    /// <summary>
    /// Navigates to the specified page using Shell navigation.
    /// </summary>
    /// <param name="pageName">The name of the page to navigate to.</param>
    /// <param name="parameters">Optional parameters to pass to the page.</param>
    public async Task NavigateToAsync(string pageName, IDictionary<string, object>? parameters = null)
    {
        try

        {
            if (parameters != null)
            {
                await Shell.Current.GoToAsync(pageName, parameters);
            }
            else
            {
                await Shell.Current.GoToAsync(pageName);
            }
        }
        catch (Exception ex)
        {
            // Log the navigation error
            System.Diagnostics.Debug.WriteLine($"Navigation error: {ex.Message}");
        }
    }

    /// <summary>
    /// Goes back to the previous page.
    /// </summary>
    public async Task GoBackAsync()
    {
        try
        {
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            // Log the navigation error
            System.Diagnostics.Debug.WriteLine($"Navigation error: {ex.Message}");
        }
    }
}
