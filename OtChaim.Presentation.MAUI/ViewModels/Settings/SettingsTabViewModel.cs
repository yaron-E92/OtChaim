using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtChaim.Presentation.MAUI.Pages.Settings;
using System.Collections.ObjectModel;

namespace OtChaim.Presentation.MAUI.ViewModels.Settings;

public partial class SettingsTabViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<ContentView> _settingsPages;

    [ObservableProperty]
    private ContentView _currentPage;

    [ObservableProperty]
    private int _currentPageIndex;

    private readonly string[] _pageTitles = ["User Info", "Medical Info", "Emergency Contacts"];

    [ObservableProperty]
    private string _currentPageTitle;

    public SettingsTabViewModel(UserInfoPage userInfoPage, MedicalInfoPage medicalInfoPage, EmergencyContactsPage emergencyContactsPage)
    {
        ArgumentNullException.ThrowIfNull(userInfoPage);
        ArgumentNullException.ThrowIfNull(medicalInfoPage);
        ArgumentNullException.ThrowIfNull(emergencyContactsPage);

        // Initialize the settings pages using injected dependencies
        SettingsPages =
        [
            userInfoPage,
            medicalInfoPage,
            emergencyContactsPage
        ];

        // Set the first page as current
        CurrentPageIndex = 0;
        CurrentPage = SettingsPages.FirstOrDefault()!;
        CurrentPageTitle = _pageTitles[CurrentPageIndex];
    }

    [RelayCommand]
    private void PreviousPage()
    {
        if (CanGoPrevious())
        {
            CurrentPageIndex--;
            CurrentPage = SettingsPages[CurrentPageIndex];
            CurrentPageTitle = _pageTitles[CurrentPageIndex];
        }
    }

    [RelayCommand]
    private void NextPage()
    {
        if (CanGoNext())
        {
            CurrentPageIndex++;
            CurrentPage = SettingsPages[CurrentPageIndex];
            CurrentPageTitle = _pageTitles[CurrentPageIndex];
        }
    }

    private bool CanGoPrevious() => CurrentPageIndex > 0;
    private bool CanGoNext() => CurrentPageIndex < SettingsPages.Count - 1;
}
