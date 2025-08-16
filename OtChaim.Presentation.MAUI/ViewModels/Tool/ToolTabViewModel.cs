using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtChaim.Presentation.MAUI.Pages.Tool;
using System.Collections.ObjectModel;

namespace OtChaim.Presentation.MAUI.ViewModels.Tool;

public partial class ToolTabViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<ContentView> _toolPages;

    [ObservableProperty]
    private ContentView _currentPage;

    [ObservableProperty]
    private int _currentPageIndex;

    private readonly string[] _pageTitles = ["Dashboard", "Emergency", "Group Status"];

    [ObservableProperty]
    private string _currentPageTitle;

    public ToolTabViewModel(EmergencyDashboardPage emergencyDashboardPage, EmergencyPage emergencyPage, GroupStatusPage groupStatusPage)
    {
        ArgumentNullException.ThrowIfNull(emergencyDashboardPage);
        ArgumentNullException.ThrowIfNull(emergencyPage);
        ArgumentNullException.ThrowIfNull(groupStatusPage);

        // Initialize the tool pages using injected dependencies
        ToolPages =
        [
            emergencyDashboardPage,
            emergencyPage,
            groupStatusPage
        ];

        // Set the first page as current
        CurrentPageIndex = 0;
        CurrentPage = ToolPages.FirstOrDefault()!;
        CurrentPageTitle = _pageTitles[CurrentPageIndex];
    }

    [RelayCommand]
    private void PreviousPage()
    {
        if (CanGoPrevious())
        {
            CurrentPageIndex--;
            CurrentPage = ToolPages[CurrentPageIndex];
            CurrentPageTitle = _pageTitles[CurrentPageIndex];
        }
    }

    [RelayCommand]
    private void NextPage()
    {
        if (CanGoNext())
        {
            CurrentPageIndex++;
            CurrentPage = ToolPages[CurrentPageIndex];
            CurrentPageTitle = _pageTitles[CurrentPageIndex];
        }
    }

    private bool CanGoPrevious() => CurrentPageIndex > 0;
    private bool CanGoNext() => CurrentPageIndex < ToolPages.Count - 1;
}
