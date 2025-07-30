using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using OtChaim.Presentation.MAUI.Pages.Settings;

namespace OtChaim.Presentation.MAUI.ViewModels.Settings;

public class SettingsTabViewModel : INotifyPropertyChanged
{
    private ObservableCollection<ContentView> _settingsPages;
    private ContentView _currentPage;

    public ObservableCollection<ContentView> SettingsPages
    {
        get => _settingsPages;
        set
        {
            _settingsPages = value;
            OnPropertyChanged();
        }
    }

    public ContentView CurrentPage
    {
        get => _currentPage;
        set
        {
            _currentPage = value;
            OnPropertyChanged();
        }
    }

    public SettingsTabViewModel()
    {
        // Initialize the settings pages
        SettingsPages =
        [
            new UserInfoPage(),
            new MedicalInfoPage(),
            new EmergencyContactsPage()
        ];

        // Set the first page as current
        CurrentPage = SettingsPages.FirstOrDefault();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
