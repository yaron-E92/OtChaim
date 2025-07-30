using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using OtChaim.Presentation.MAUI.Pages.Tool;

namespace OtChaim.Presentation.MAUI.ViewModels.Tool;

public class ToolTabViewModel : INotifyPropertyChanged
{
    private ObservableCollection<ContentView> _toolPages;
    private ContentView _currentPage;

    public ObservableCollection<ContentView> ToolPages
    {
        get => _toolPages;
        set
        {
            _toolPages = value;
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

    public ToolTabViewModel()
    {
        // Initialize the tool pages
        ToolPages =
        [
            new EmergencyPage(),
            new GroupStatusPage()
        ];

        // Set the first page as current
        CurrentPage = ToolPages.FirstOrDefault();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
