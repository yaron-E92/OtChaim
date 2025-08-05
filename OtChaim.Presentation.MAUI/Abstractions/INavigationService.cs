namespace OtChaim.Presentation.MAUI.Abstractions;

/// <summary>
/// Service for handling navigation between pages in the application.
/// </summary>
public interface INavigationService
{
    /// <summary>
    /// Navigates to the specified page.
    /// </summary>
    /// <param name="pageName">The name of the page to navigate to.</param>
    /// <param name="parameters">Optional parameters to pass to the page.</param>
    Task NavigateToAsync(string pageName, IDictionary<string, object>? parameters = null);

    /// <summary>
    /// Goes back to the previous page.
    /// </summary>
    Task GoBackAsync();
}
